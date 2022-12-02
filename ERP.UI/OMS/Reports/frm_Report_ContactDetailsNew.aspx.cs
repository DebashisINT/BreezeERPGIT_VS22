using System;
using System.Data;
using System.IO;
using System.Web;
using System.Web.UI;
using BusinessLogicLayer;
using CrystalDecisions.CrystalReports.Engine;


namespace ERP.OMS.Reports
{
    public partial class Reports_frm_Report_ContactDetailsNew : System.Web.UI.Page, System.Web.UI.ICallbackEventHandler
    {
        MasterReports mr = new MasterReports();
        DataTable dtClients = new DataTable();
        string data;
        //BusinessLogicLayer.Converter OConvert = new BusinessLogicLayer.Converter();
        BusinessLogicLayer.Converter oconverter = new BusinessLogicLayer.Converter();
        DataSet ds = new DataSet();
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        int pageindex = 0;
        DataTable dtgrp = new DataTable();
        int PageNo = 0;
        DataTable DtMain = new DataTable();
        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //'http://localhost:2957/InfluxCRM/management/testProjectMainPage_employee.aspx'
                string sPath = HttpContext.Current.Request.Url.ToString();
                oDBEngine.Call_CheckPageaccessebility(sPath);
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {


            if (HttpContext.Current.Session["userid"] == null)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }
            //this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>height();</script>");
            Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script language='javascript'>Page_Load();</script>");

            dtFrom.EditFormatString = oconverter.GetDateFormat("Date");
            dtTo.EditFormatString = oconverter.GetDateFormat("Date");
            if (!IsPostBack)
            {

                dtFrom.Value = Convert.ToDateTime(DateTime.Today);
                dtTo.Value = Convert.ToDateTime(DateTime.Today);

                settno();

            }
            //_____For performing operation without refreshing page___//

            String cbReference = Page.ClientScript.GetCallbackEventReference(this, "arg", "ReceiveServerData", "context");
            String callbackScript = "function CallServer(arg, context){ " + cbReference + ";}";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "CallServer", callbackScript, true);
            //___________-end here___//
            //Page.ClientScript.RegisterStartupScript(GetType(), "JScript", "<script language='javascript'>divscroll('" + HttpContext.Current.Session["ExchangeSegmentID"] + "');</script>");


        }
        string ICallbackEventHandler.GetCallbackResult()
        {
            return data;
        }
        void ICallbackEventHandler.RaiseCallbackEvent(string eventArgument)
        {
            string id = eventArgument.ToString();
            string[] idlist = id.Split('~');
            string str = "";
            string str1 = "";
            if (idlist[0] == "ComboChange")
            {
                //MainAcID = idlist[1];
            }
            else
            {
                string[] cl = idlist[1].Split(',');
                for (int i = 0; i < cl.Length; i++)
                {
                    if (idlist[0] != "Clients")
                    {
                        if (idlist[0] == "SettlementType")
                        {
                            string[] val = cl[i].Split(';');
                            if (str == "")
                            {
                                str = "'" + val[0] + "'";
                                str1 = "'" + val[0] + "'" + ";" + val[1];
                            }
                            else
                            {
                                str += ",'" + val[0] + "'";
                                str1 += "," + "'" + val[0] + "'" + ";" + val[1];
                            }
                        }
                        else
                        {
                            string[] val = cl[i].Split(';');
                            if (str == "")
                            {
                                str = val[0];
                                str1 = val[0] + ";" + val[1];
                            }
                            else
                            {
                                str += "," + val[0];
                                str1 += "," + val[0] + ";" + val[1];
                            }
                        }
                    }
                    else
                    {
                        string[] val = cl[i].Split(';');
                        string[] AcVal = val[0].Split('-');
                        if (str == "")
                        {
                            str = "'" + AcVal[0] + "'";
                            str1 = "'" + AcVal[0] + "'" + ";" + val[1];
                        }
                        else
                        {
                            str += ",'" + AcVal[0] + "'";
                            str1 += "," + "'" + AcVal[0] + "'" + ";" + val[1];
                        }
                    }
                }

                if (idlist[0] == "Branch")
                {
                    data = "Branch~" + str;
                }
                else if (idlist[0] == "Segment")
                {
                    data = "Segment~" + str1;
                }

                else if (idlist[0] == "Clients")
                {
                    data = "Clients~" + str;
                }
                else if (idlist[0] == "Group")
                {
                    data = "Group~" + str;
                }
            }
        }
        protected void btnhide_Click(object sender, EventArgs e)
        {
            if (ddlGroup.SelectedItem.Value.ToString() == "1")
            {
                if (HiddenField_Group.Value.ToString().Trim() == "")
                {
                    BindGroup();
                }
            }
        }

        void settno()
        {
            DataTable DtSegComp = oDBEngine.GetDataTable("(select exch_compId,exch_internalId,isnull((select exh_shortName from tbl_master_Exchange where exh_cntId=tbl_master_companyExchange.exch_exchId)+'-'+exch_segmentId,exch_membershiptype) as Comp,isnull((select exh_shortName from tbl_master_Exchange where exh_cntId=tbl_master_companyExchange.exch_exchId)+' - '+exch_segmentId,exch_membershiptype) as Comp1 from tbl_master_companyExchange where exch_compid='" + Session["LastCompany"].ToString() + "') as D", "*", "Comp in(select seg_name from tbl_master_segment where seg_id=" + Session["userlastsegment"].ToString() + ")");
            if (DtSegComp.Rows.Count > 0)
            {
                litSegment.InnerText = DtSegComp.Rows[0][3].ToString(); ///Segment disply within braket
                HiddenField_SegmentName.Value = DtSegComp.Rows[0][3].ToString();
                HiddenField_Segment.Value = DtSegComp.Rows[0][1].ToString();

            }


        }
        public void BindGroup()
        {
            ddlgrouptype.Items.Clear();
            DataTable DtGroup = oDBEngine.GetDataTable("tbl_master_groupmaster", "distinct gpm_Type", null);
            if (DtGroup.Rows.Count > 0)
            {
                ddlgrouptype.DataSource = DtGroup;
                ddlgrouptype.DataTextField = "gpm_Type";
                ddlgrouptype.DataValueField = "gpm_Type";
                ddlgrouptype.DataBind();
                //ddlgrouptype.Items.Insert(0, new ListItem("Select GroupType", "0"));
                DtGroup.Dispose();

            }
        }

        public void CreateDataTable()
        {

            //string startdate = dtFrom.Date.Month.ToString() + "/" + dtFrom.Date.Day.ToString() + "/" + dtFrom.Date.Year.ToString() + " 00:01 AM";
            //string Enddate = dtTo.Date.Month.ToString() + "/" + dtTo.Date.Day.ToString() + "/" + dtTo.Date.Year.ToString() + " 11:59 PM";

            string startdate = dtFrom.Date.Month.ToString() + "/" + dtFrom.Date.Day.ToString() + "/" + dtFrom.Date.Year.ToString() + " 00:00:00.000";
            string Enddate = dtTo.Date.Month.ToString() + "/" + dtTo.Date.Day.ToString() + "/" + dtTo.Date.Year.ToString() + " 00:00:00.000";

            string Query1 = "";
            if (rdbClientALL.Checked)
            {

                if (ddlGroup.SelectedItem.Value.ToString() == "0")
                {
                    if (rdbranchAll.Checked)
                    {
                        Query1 = "select cnt_internalid from tbl_master_contact  where cnt_internalid like 'CL%'";
                    }
                    else
                    {

                        Query1 = "select cnt_internalid from tbl_master_contact  where cnt_internalid like 'CL%' and cnt_branchid in (" + HiddenField_Branch.Value.ToString().Trim() + ")";
                    }
                }
                else
                {

                    if (rdddlgrouptypeAll.Checked)
                    {

                        Query1 = "select cnt_internalid from tbl_master_contact  where cnt_internalid like 'CL%'";
                    }
                    else
                    {

                        Query1 = "select grp_contactid from tbl_trans_group  where grp_contactid like 'CL%' and grp_groupmaster in (" + HiddenField_Group.Value.ToString().Trim() + ")";
                    }
                }
                if (rdbSegAll.Checked)
                {

                    DtMain = oDBEngine.GetDataTable("tbl_master_contact", "cnt_internalid", "cnt_internalid like 'CL%'");

                }
                else
                {
                    DtMain = oDBEngine.GetDataTable("tbl_master_contactexchange,tbl_master_contact", " distinct cnt_internalId", "crg_cntid=cnt_internalid   and crg_exchange in (select exh_shortName+' '+ '-' +' '+exch_segmentId as SegmentName from tbl_master_companyExchange,tbl_master_Exchange where exch_internalId in(" + HiddenField_Segment.Value.ToString().Trim() + ") and exch_compid='" + Session["LastCompany"].ToString() + "' and exh_cntId=exch_exchId) and cnt_internalId in (" + Query1 + ") and (CAST(crg_regisDate AS datetime) >= CONVERT(varchar,'" + startdate + "', 101)) and (CAST(crg_regisDate AS datetime) <= CONVERT(varchar,'" + Enddate + "', 101))");

                }
            }
            else
            {
                DtMain = oDBEngine.GetDataTable("tbl_master_contact", "cnt_internalid", "cnt_internalid in (" + HiddenField_Client.Value.ToString().Trim() + ")");
            }
            Session["MainDataTable"] = DtMain;
        }

        public void fillHTML()
        {



            DataTable dtGeneral = oDBEngine.GetDataTable("tbl_master_contact", "top 1 isnull(ltrim(rtrim(cnt_ucc)),'') as [ClientCode]," +
                        "isnull(ltrim(rtrim(cnt_firstname)),'')+' '+ isnull(ltrim(rtrim(cnt_middlename)),'') +' '+isnull(ltrim(rtrim(cnt_lastname)),'')  as [ClientName] ," +
                        "(select  lgl_legalStatus  from tbl_master_legalstatus where lgl_id=cnt_legalStatus ) as [Category]," +
                        "(select  cntstu_contactstatus  from tbl_master_contactStatus where cntstu_id=cnt_contactStatus ) as [Status]," +
                        "(select top 1 isnull(crg_Number,' ') from tbl_master_contactRegistration where crg_cntID=cnt_internalID and crg_type='Pancard') as [PanNo]," +
                        "(select top 1 eml_email from tbl_master_email where  eml_cntId=cnt_internalID and eml_type='Official') as [ClientEmail]," +
                        "(select top 1  isnull(ltrim(rtrim(phf_countryCode)),'') from tbl_master_phonefax  where phf_cntId=cnt_internalID) as [ISDCode]," +
                        "(select top 1  isnull(ltrim(rtrim(phf_areaCode)),'') from tbl_master_phonefax  where phf_cntId=cnt_internalID) as [STDCode]," +
                        "(select top 1  isnull(ltrim(rtrim(phf_phoneNumber)),'') from tbl_master_phonefax  where phf_cntId=cnt_internalID and phf_type<>'Mobile') as [TelephoneNumber]," +
                        "(select top 1  isnull(ltrim(rtrim(phf_phoneNumber)),'') from tbl_master_phonefax  where phf_cntId=cnt_internalID and phf_type='Mobile') as [MobNumber]," +
                        "REPLACE(CONVERT(VARCHAR(11), cnt_dOB, 106), ' ', '-')  as DateOfBirth," +
                        "(select top 1 REPLACE(CONVERT(VARCHAR(11), crg_regisDate, 106), ' ', '-') from tbl_master_contactExchange  where crg_cntID=cnt_internalID) as ClientAgrementDate," +
                        "(select top 1 isnull(crg_Number,' ') from tbl_master_contactRegistration where crg_cntID=cnt_internalID and crg_type='Other' and crg_registrationAuthority='ROC') as  RegNo," +
                        "(select top 1 isnull(crg_registrationAuthority,' ') from tbl_master_contactRegistration where crg_cntID=cnt_internalID and crg_type='Other' and crg_registrationAuthority='ROC') as  RegAuthority," +
                        "(select top 1 isnull(crg_place,' ') from tbl_master_contactRegistration where crg_cntID=cnt_internalID and crg_type='Other' and crg_registrationAuthority='ROC') as  RegPlace," +
                        "(select top 1 isnull(REPLACE(CONVERT(VARCHAR(11), cnt_dOB, 106), ' ', '-'),' ') from tbl_master_contactRegistration where crg_cntID=cnt_internalID and crg_type='Other' and crg_registrationAuthority='ROC') as RegistrationDate", "cnt_internalID='" + DtMain.Rows[PageNo]["cnt_internalid"].ToString().Trim() + "'");

            DataTable DtAdd = new DataTable();
            DtAdd = oDBEngine.GetDataTable("tbl_master_address", " isnull(ltrim(rtrim(add_address1)),'')  as Addres1, isnull(ltrim(rtrim(add_address2)),'')  as Addres2, isnull(ltrim(rtrim(add_address3)),'') as Addres3,(select isnull(ltrim(rtrim(city_name)),'') from tbl_master_City  where city_id =add_City) as City,(select isnull(ltrim(rtrim(state)),'') from tbl_master_state  where id= add_state) as State,(select isnull(ltrim(rtrim(cou_country)),'') from tbl_master_country  where cou_id=add_country ) as Country, isnull(ltrim(rtrim(add_pin)),'')  as Pin", " add_addressType='Registered'  and  add_cntId='" + DtMain.Rows[PageNo]["cnt_internalid"].ToString().Trim() + "'");
            if (DtAdd.Rows.Count == 0)
            {
                DtAdd = oDBEngine.GetDataTable("tbl_master_address", "top 1 isnull(ltrim(rtrim(add_address1)),'')  as Addres1, isnull(ltrim(rtrim(add_address2)),'')  as Addres2, isnull(ltrim(rtrim(add_address3)),'') as Addres3,(select isnull(ltrim(rtrim(city_name)),'') from tbl_master_City  where city_id =add_City) as City,(select isnull(ltrim(rtrim(state)),'') from tbl_master_state  where id= add_state) as State,(select isnull(ltrim(rtrim(cou_country)),'') from tbl_master_country  where cou_id=add_country ) as Country, isnull(ltrim(rtrim(add_pin)),'')  as Pin", " add_cntId='" + DtMain.Rows[PageNo]["cnt_internalid"].ToString().Trim() + "'");

            }

            DataTable dp = oDBEngine.GetDataTable("tbl_master_contactDPDetails", "dpd_accountType as AccountType,case when dpd_POA=1 then 'YES' else '' end as IsPOA, dpd_dpCode as dipositoryID,(select dp_dpName from tbl_master_depositoryParticipants  where left(dp_dpid,8)=left(dpd_dpCode,8)) as DipositoryName ,dpd_ClientId as  BenOwnerAccNo", "dpd_cntID='" + DtMain.Rows[PageNo]["cnt_internalid"].ToString().Trim() + "'");
            DataTable dpcm = oDBEngine.GetDataTable("tbl_master_contactDPDetails", "dpd_accountType as AccountType,case when dpd_POA=1 then 'YES' else '' end as IsPOA, dpd_dpCode as dipositoryID,(select dp_dpName from tbl_master_depositoryParticipants  where left(dp_dpid,8)=left(dpd_dpCode,8)) as DipositoryName ,dpd_ClientId as  BenOwnerAccNo", "dpd_cntID='" + DtMain.Rows[PageNo]["cnt_internalid"].ToString().Trim() + "'  and (dpd_accounttype='Default' or dpd_accounttype='Secondary') ");
            DataTable dpcomm = oDBEngine.GetDataTable("tbl_master_contactDPDetails", "dpd_accountType as AccountType,case when dpd_POA=1 then 'YES' else '' end as IsPOA, dpd_dpCode as dipositoryID,(select dp_dpName from tbl_master_depositoryParticipants  where left(dp_dpid,8)=left(dpd_dpCode,8)) as DipositoryName ,dpd_ClientId as  BenOwnerAccNo", "dpd_cntID='" + DtMain.Rows[PageNo]["cnt_internalid"].ToString().Trim() + "'and (dpd_accounttype='CommodityDP' or dpd_accounttype='CommodityDP Sec') ");

            DataTable dtBank = oDBEngine.GetDataTable("tbl_trans_contactBankDetails ,tbl_master_Bank ", " cbd_accountCategory as Category, tbl_master_Bank.bnk_bankName AS BankName,(tbl_master_Bank.bnk_bankName + ' ' + tbl_master_Bank.bnk_branchName + ' ' + tbl_master_Bank.bnk_micrno ) AS BranchAdress,  tbl_trans_contactBankDetails.cbd_accountType  as AccountType, tbl_trans_contactBankDetails.cbd_accountNumber AS AccountNumber, tbl_master_Bank.bnk_micrno as BankMICR,tbl_master_Bank.bnk_IFSCCODE as BankIFSC", "(tbl_trans_contactBankDetails.cbd_cntId ='" + DtMain.Rows[PageNo]["cnt_internalid"].ToString().Trim() + "') and  tbl_master_Bank.bnk_id=tbl_trans_contactBankDetails.cbd_bankCode");
            DataTable dtProof = oDBEngine.GetDataTable("tbl_master_contactRegistration", " crg_type, crg_Number,crg_place,crg_Date", "crg_cntID='" + DtMain.Rows[PageNo]["cnt_internalid"].ToString().Trim() + "'");
            DataTable dtGroup = oDBEngine.GetDataTable(" tbl_trans_group INNER JOIN tbl_master_groupMaster ON tbl_trans_group.grp_groupMaster = tbl_master_groupMaster.gpm_id ", "tbl_trans_group.grp_id as ID,ltrim(rtrim(tbl_master_groupMaster.gpm_Description)) +'['+rtrim(ltrim(tbl_master_groupMaster.gpm_code))+']' as GroupName, tbl_master_groupMaster.gpm_Type as GroupType ", "tbl_trans_group.grp_contactId='" + DtMain.Rows[PageNo]["cnt_internalid"].ToString().Trim() + "'");
            DataTable dtSeg = new DataTable();
            DataTable dtCPD = new DataTable();
            dtCPD = oDBEngine.GetDataTable("select  A.cp_contactId as ContactId, A.cp_name as name,(select phf_phonenumber from tbl_master_phonefax where phf_cntId=A.cp_contactId and phf_type='Office') as Officephone," +
                           "(select phf_phonenumber from tbl_master_phonefax where phf_cntId=A.cp_contactId and phf_type='Residence') as Residencephone," +
                           "(select phf_phonenumber from tbl_master_phonefax where phf_cntId=A.cp_contactId and phf_type='Mobile') as Mobilephone," +
                           "isnull(('(O)'+(select phf_phonenumber from tbl_master_phonefax where phf_cntId=A.cp_contactId and phf_type='Office')),'')+" +
                           "isnull(('(R)'+(select phf_phonenumber from tbl_master_phonefax where phf_cntId=A.cp_contactId and phf_type='Residence')),'')+" +
                           "isnull(('(M)'+(select phf_phonenumber from tbl_master_phonefax where phf_cntId=A.cp_contactId and phf_type='Mobile')),'') as Phone," +
                           "(select eml_email from tbl_master_email where eml_cntId=A.cp_contactId) as email, (case when cp_status = 'Y' then 'Active' " +
                           "when cp_status = 'N' then 'Suspended' else 'N/A'  end) as status, ltrim(rtrim(cp_status)) as cp_status," +
                           "(select deg_designation from tbl_master_designation where deg_id= ltrim(rtrim(  cp_designation))) as  cp_designation," +
                           "(select fam_familyRelationship from tbl_master_familyrelationship where fam_id=ltrim(rtrim( cp_relationShip))) as cp_relationShip," +
                           "cp_Pan,cp_Din from tbl_master_contactperson A  where cp_agentInternalId in('" + DtMain.Rows[PageNo]["cnt_internalid"].ToString().Trim() + "') ORDER BY cp_status desc");



            if (rdbSegAll.Checked == true)
            {
                dtSeg = oDBEngine.GetDataTable("tbl_master_contactExchange ce ", "ce.crg_internalId as crg_internalId,ce.crg_cntID as crg_cntID,ce.crg_exchange as crg_exchange1,ce.crg_company as crg_company1,case crg_company when '0' then 'N/A' else (select cmp_name from tbl_master_company where cmp_internalId=ce.crg_company) end as crg_company,ce.crg_exchange as crg_exchange,ltrim(rtrim(ce.crg_tcode)) as crg_tcode,case when ce.crg_regisDate='1/1/1900 12:00:00 AM' then null else convert(varchar(11),ce.crg_regisDate,106) end as crg_regisDate1,case when ce.crg_regisDate='1/1/1900 12:00:00 AM' then null else convert(varchar(11),ce.crg_regisDate,106) end as crg_regisDate,case when ce.crg_businessCmmDate='1/1/1900 12:00:00 AM' then null else convert(varchar(11),ce.crg_businessCmmDate,113) end as crg_businessCmmDate1,case when ce.crg_businessCmmDate='1/1/1900 12:00:00 AM' then null else convert(varchar(11),ce.crg_businessCmmDate,113) end as crg_businessCmmDate,case when ce.crg_suspensionDate='1/1/1900 12:00:00 AM' then null else convert(varchar(11),ce.crg_suspensionDate,113) end as crg_suspensionDate1,case when ce.crg_suspensionDate='1/1/1900 12:00:00 AM' then null else convert(varchar(11),ce.crg_suspensionDate,113) end as crg_suspensionDate,ltrim(rtrim(ce.crg_reasonforSuspension)) as crg_reasonforSuspension,ce.CreateUser as CreateUser,ltrim(rtrim(ce.crg_SubBrokerFranchiseeID)) as crg_SubBrokerFranchiseeID,ltrim(rtrim(ce.crg_Dealer)) as crg_Dealer,case when ce.crg_AccountClosureDate='1/1/1900 12:00:00 AM' then null else cast(ce.crg_AccountClosureDate as datetime) end as crg_AccountClosureDate,ltrim(rtrim(ce.crg_FrontOfficeBranchCode)) as crg_FrontOfficeBranchCode,ltrim(rtrim(ce.crg_FrontOfficeGroupCode)) as crg_FrontOfficeGroupCode,ltrim(rtrim(ce.crg_ParticipantSchemeCode)) as crg_ParticipantSchemeCode,ltrim(rtrim(ce.crg_ClearingBankCode)) as crg_ClearingBankCode,ltrim(rtrim(ce.crg_SchemeCode)) as crg_SchemeCode,ltrim(rtrim(ce.crg_STTPattern)) as crg_STTPattern,(select isnull(ltrim(rtrim(cnt_firstname)),'')+' '+isnull(ltrim(rtrim(cnt_middlename)),'')+' '+isnull(ltrim(rtrim(cnt_lastname)),'')+' ['+isnull(ltrim(rtrim(cnt_shortName)),'')+']' from tbl_master_contact where cnt_internalId=ce.crg_SubBrokerFranchiseeID) as Franchisee,(select isnull(ltrim(rtrim(cnt_firstname)),'')+' '+isnull(ltrim(rtrim(cnt_middlename)),'')+' '+isnull(ltrim(rtrim(cnt_lastname)),'')+' ['+isnull(ltrim(rtrim(cnt_ucc)),'')+']' from tbl_master_contact where cnt_internalId=ce.crg_Dealer) as Dealer ", "crg_cntID='" + DtMain.Rows[PageNo]["cnt_internalid"].ToString().Trim() + "'  and crg_company='" + Session["LastCompany"].ToString() + "'");
            }
            else
            {
                string Seg = "'N'";
                string[] SegmentName = HiddenField_SegmentName.Value.ToString().Split(',');
                for (int n = 0; n < SegmentName.Length; n++)
                {
                    Seg = Seg + ",'" + SegmentName[n] + "'";

                }
                dtSeg = oDBEngine.GetDataTable("tbl_master_contactExchange ce ", "ce.crg_exchange as crg_exchange,ce.crg_company as crg_company,case crg_company when '0' then 'N/A' else (select cmp_name from tbl_master_company where cmp_internalId=ce.crg_company) end as crg_company,ltrim(rtrim(ce.crg_tcode)) as crg_tcode,case when ce.crg_regisDate='1/1/1900 12:00:00 AM' then null else convert(varchar(11),ce.crg_regisDate,106) end as crg_regisDate,case when ce.crg_businessCmmDate='1/1/1900 12:00:00 AM' then null else convert(varchar(11),ce.crg_businessCmmDate,106) end as crg_businessCmmDate,case when ce.crg_suspensionDate='1/1/1900 12:00:00 AM' then null else convert(varchar(11),ce.crg_suspensionDate,106) end as crg_suspensionDate ", "crg_cntID='" + DtMain.Rows[PageNo]["cnt_internalid"].ToString().Trim() + "' and  crg_exchange in (" + Seg + ")  ");
            }

            DataTable dtHdr = new DataTable();
            DataTable dtFtr = new DataTable();
            if (chkHeader.Checked == true)
            {
                dtHdr = oDBEngine.GetDataTable("Master_HeaderFooter", "HeaderFooter_Content", "HeaderFooter_ID='" + txtHeader_hidden.Value + "'");
            }



            string dspTbl = "";
            if (dtGeneral.Rows.Count == 0)
            {

                display.InnerHtml = "<table width=\"100%\"><tr><td align=\"center\"><b>Nor Record Found:</td></tr></table>";

            }
            else
            {
                string dispTbl = "";
                dispTbl = "<table   cellpadding=\"1\" cellspacing=\"1\" width=\"100%\" style=\"background-color: #ffffff; font-family: verdana; font-size: 12px;color:White;border: 1px solid black;\" >";
                if (chkHeader.Checked == true)
                {
                    if (dtHdr.Rows.Count > 0)
                    {
                        dispTbl = dispTbl + "<tr style=\"background-color: #ffffff;\" ><td align=\"left\"  valign=\"top\" colspan=\"2\">" + dtHdr.Rows[0]["HeaderFooter_Content"].ToString() + "</td></tr>";
                    }
                }
                dispTbl = dispTbl + "<tr style=\"background-color: #ffffff;\" ><td align=\"left\"  valign=\"top\" colspan=\"2\">";
                dispTbl = dispTbl + "<table  cellpadding=\"1\" cellspacing=\"1\"  style=\"background-color: #ffffff; font-family: verdana; font-size: 12px;color:White;\"><tr style=\"background-color: #ffffff;\">";
                dispTbl = dispTbl + "<td style=\"padding:5px 6px 5px 6px\">";
                dispTbl = dispTbl + "<table  cellpadding=\"1\" cellspacing=\"1\" style=\"background-color: #99ccff; font-family: verdana; font-size: 12px;color:White;\">";

                dispTbl = dispTbl + "<tr><td colspan=\"2\" align=\"left\"><b>Customer Details</td></td></tr>";

                dispTbl = dispTbl + "<tr  style=\"background-color: #ffffff;\"><td style=\"background-color: #ccffff;\"><b>Name: </td><td><b>" + dtGeneral.Rows[0]["ClientName"].ToString().Trim() + "</td></tr>";
                dispTbl = dispTbl + "<tr  style=\"background-color: #ffffff;\"><td style=\"background-color: #ccffff;\"><b>Category: </td><td>" + dtGeneral.Rows[0]["Category"].ToString().Trim() + "</td></tr>";
                dispTbl = dispTbl + "<tr  style=\"background-color: #ffffff;\"><td style=\"background-color: #ccffff;\"><b>Status: </td><td>" + dtGeneral.Rows[0]["status"].ToString().Trim() + "</td></tr>";
                if (DtAdd.Rows.Count > 0)
                {
                    dispTbl = dispTbl + "<tr  style=\"background-color: #ffffff;\"><td style=\"background-color: #ccffff;\"><b>Address: </td><td>" + DtAdd.Rows[0]["Addres1"].ToString().Trim() + ' ' + DtAdd.Rows[0]["Addres2"].ToString().Trim() + ' ' + DtAdd.Rows[0]["Addres3"].ToString().Trim() + "</td></tr>";
                    dispTbl = dispTbl + "<tr  style=\"background-color: #ffffff;\"><td style=\"background-color: #ccffff;\"><b>City: </td><td>" + DtAdd.Rows[0]["City"].ToString().Trim() + "</td></tr>";
                    dispTbl = dispTbl + "<tr  style=\"background-color: #ffffff;\"><td style=\"background-color: #ccffff;\"><b>State: </td><td>" + DtAdd.Rows[0]["State"].ToString().Trim() + "</td></tr>";
                    dispTbl = dispTbl + "<tr  style=\"background-color: #ffffff;\"><td style=\"background-color: #ccffff;\"><b>Country: </td><td>" + DtAdd.Rows[0]["Country"].ToString().Trim() + "</td></tr>";
                    dispTbl = dispTbl + "<tr  style=\"background-color: #ffffff;\"><td style=\"background-color: #ccffff;\"><b>Pin: </td><td>" + DtAdd.Rows[0]["Pin"].ToString().Trim() + "</td></tr>";

                }
                else
                {
                    dispTbl = dispTbl + "<tr  style=\"background-color: #ffffff;\"><td style=\"background-color: #ccffff;\"><b>Address: </td><td></td></tr>";
                    dispTbl = dispTbl + "<tr  style=\"background-color: #ffffff;\"><td style=\"background-color: #ccffff;\"><b>City: </td><td></td></tr>";
                    dispTbl = dispTbl + "<tr  style=\"background-color: #ffffff;\"><td style=\"background-color: #ccffff;\"><b>State: </td><td></td></tr>";
                    dispTbl = dispTbl + "<tr  style=\"background-color: #ffffff;\"><td style=\"background-color: #ccffff;\"><b>Country: </td><td></td></tr>";
                    dispTbl = dispTbl + "<tr  style=\"background-color: #ffffff;\"><td style=\"background-color: #ccffff;\"><b>Pin: </td><td></td></tr>";
                }
                dispTbl = dispTbl + "<tr  style=\"background-color: #ffffff;\"><td style=\"background-color: #ccffff;\"><b>Telephone Number: </td><td>" + dtGeneral.Rows[0]["TelephoneNumber"].ToString().Trim() + "</td></tr>";
                dispTbl = dispTbl + "<tr  style=\"background-color: #ffffff;\"><td style=\"background-color: #ccffff;\"><b>Mobile Number: </td><td>" + dtGeneral.Rows[0]["MobNumber"].ToString().Trim() + "</td></tr>";
                dispTbl = dispTbl + "<tr  style=\"background-color: #ffffff;\"><td style=\"background-color: #ccffff;\"><b>Email Id: </td><td>" + dtGeneral.Rows[0]["ClientEmail"].ToString().Trim() + "</td></tr>";

                dispTbl = dispTbl + "</table>";
                dispTbl = dispTbl + "</td>";


                dispTbl = dispTbl + "<td  style=\"padding:5px 6px 5px 6px\" valign=\"top\">";
                //-----------Identity Details --------
                dispTbl = dispTbl + "<table  cellpadding=\"1\" cellspacing=\"1\" style=\"background-color: #99ccff; font-family: verdana; font-size: 12px;color:White;\"><tr><td colspan=\"2\" align=\"left\"><b>Statutory Details</td></tr><tr  style=\"background-color: #ccffff;\"><td>Doc. Type</td><td>Number</td></tr>";
                for (int k = 0; k <= dtProof.Rows.Count - 1; k++)
                {
                    dispTbl = dispTbl + "<tr  style=\"background-color: #ffffff;\"><td>" + dtProof.Rows[k]["crg_type"].ToString() + "</td><td>" + dtProof.Rows[k]["crg_Number"].ToString() + "</td></tr>";
                }
                dispTbl = dispTbl + "</table>";
                //-----------End --------
                dispTbl = dispTbl + "</td>";



                dispTbl = dispTbl + "<td style=\"padding:5px 6px 5px 6px\" valign=\"top\">";
                //-----------Unique Code--------
                dispTbl = dispTbl + "<table  cellpadding=\"1\" cellspacing=\"1\"  style=\"background-color: #99ccff; font-family: verdana; font-size: 12px;color:White;\"><tr><td colspan=\"2\" align=\"left\"><b>Exchange Details</td></tr><tr  style=\"background-color: #ccffff;\"><td>Segment</td><td>Code</td><td>Registration Date</td><td>BusinessComm. Date</td><td>Suspension Date</td></tr>";
                for (int i = 0; i <= dtSeg.Rows.Count - 1; i++)
                {
                    dispTbl = dispTbl + "<tr  style=\"background-color: #ffffff;\"><td>" + dtSeg.Rows[i]["crg_exchange"].ToString() + "</td><td>" + dtSeg.Rows[i]["crg_tcode"].ToString() + "</td><td> " + dtSeg.Rows[i]["crg_regisDate"].ToString() + "</td><td> " + dtSeg.Rows[i]["crg_businessCmmDate"].ToString() + "</td><td> " + dtSeg.Rows[i]["crg_suspensionDate"].ToString() + "</td></tr>";
                }
                dispTbl = dispTbl + "</table>";
                //-----------End --------
                dispTbl = dispTbl + "</td>";



                dispTbl = dispTbl + "</tr>";
                dispTbl = dispTbl + "</table>";
                dispTbl = dispTbl + "</td></tr>";








                dispTbl = dispTbl + "<tr style=\"background-color: #ffffff;\" ><td align=\"left\"  valign=\"top\" colspan=\"2\">";
                dispTbl = dispTbl + "<table  cellpadding=\"1\" cellspacing=\"1\"  style=\"background-color: #ffffff; font-family: verdana; font-size: 12px;color:White;\"><tr style=\"background-color: #ffffff;\">";

                dispTbl = dispTbl + "<td style=\"padding:5px 6px 5px 6px\" valign=\"top\">";
                //-----------Group Details --------
                dispTbl = dispTbl + "<table  cellpadding=\"1\" cellspacing=\"1\" style=\"background-color: #99ccff; font-family: verdana; font-size: 12px;color:White;\"><tr><td colspan=\"2\" align=\"left\"><b>Group Details</td></tr><tr  style=\"background-color: #ccffff;\"><td>Group Type</td><td>Name</td></tr>";
                for (int j = 0; j <= dtGroup.Rows.Count - 1; j++)
                {
                    dispTbl = dispTbl + "<tr  style=\"background-color: #ffffff;\"><td>" + dtGroup.Rows[j]["GroupType"].ToString() + "</td><td>" + dtGroup.Rows[j]["GroupName"].ToString() + "</td></tr>";
                }
                dispTbl = dispTbl + "</table>";
                //-----------End --------
                dispTbl = dispTbl + "</td>";



                //dispTbl = dispTbl + "<td style=\"padding:5px 6px 5px 6px\" valign=\"top\">";
                //dispTbl = dispTbl + "<table  cellpadding=\"1\" cellspacing=\"1\"  style=\"background-color: #99ccff; font-family: verdana; font-size: 12px;color:White;\"><tr><td colspan=\"3\" align=\"left\"><b>DP Details</td></tr><tr  style=\"background-color: #ccffff;\"><td>Category</td><td>Dipository ID</td><td>Name </td><td>Account ID</td><td>&nbsp;Is POA</td></tr>";
                if (rdbSegAll.Checked == false)
                {
                    dispTbl = dispTbl + "<td style=\"padding:5px 6px 5px 6px\" valign=\"top\">";
                    dispTbl = dispTbl + "<table  cellpadding=\"1\" cellspacing=\"1\"  style=\"background-color: #99ccff; font-family: verdana; font-size: 12px;color:White;\"><tr><td colspan=\"3\" align=\"left\"><b>DP Details</td></tr><tr  style=\"background-color: #ccffff;\"><td>Category</td><td>Dipository ID</td><td>Name </td><td>Account ID</td><td>&nbsp;Is POA</td></tr>";
                    bool segfordp1 = false;
                    segfordp1 = ((HiddenField_SegmentName.Value.ToString().Contains("SPOT") || HiddenField_SegmentName.Value.ToString().Contains("COMM"))
                        && (HiddenField_SegmentName.Value.ToString().Contains("CM") || HiddenField_SegmentName.Value.ToString().Contains("FO") || HiddenField_SegmentName.Value.ToString().Contains("CDX")));
                    if (dp.Rows.Count > 0 && segfordp1)
                    {
                        for (int x = 0; x <= dp.Rows.Count - 1; x++)
                        {
                            dispTbl = dispTbl + "<tr  style=\"background-color: #ffffff;\"><td>&nbsp;" + dp.Rows[x]["AccountType"].ToString() + "</td><td>" + dp.Rows[x]["dipositoryID"].ToString() + "</td><td>" + dp.Rows[x]["DipositoryName"].ToString() + "</td><td>" + dp.Rows[x]["BenOwnerAccNo"].ToString() + "</td><td>&nbsp;" + dp.Rows[x]["IsPOA"].ToString() + "</td></tr>";
                        }
                        //dispTbl = dispTbl + "</table>";
                        dispTbl = dispTbl + "</td>";


                        //dispTbl = dispTbl + "</table>";
                        dispTbl = dispTbl + "</td></tr>";





                        //dispTbl = dispTbl + "<tr style=\"background-color: #ffffff;\" ><td align=\"left\"  valign=\"top\" colspan=\"2\">";
                        //dispTbl = dispTbl + "<table  cellpadding=\"1\" cellspacing=\"1\"  style=\"background-color: #ffffff; font-family: verdana; font-size: 12px;color:White;\"><tr style=\"background-color: #ffffff;\">";
                        //dispTbl = dispTbl + "<td style=\"padding:5px 6px 5px 6px\">";
                        //dispTbl = dispTbl + "<table  cellpadding=\"1\" cellspacing=\"1\" style=\"background-color: #99ccff; font-family: verdana; font-size: 12px;color:White;\"><tr><td colspan=\"6\" align=\"left\"><b>Bank Details</td></tr><tr  style=\"background-color: #ccffff;\"><td>Category</td><td>Bank Name</td><td>MICR No.</td><td>IFSC Code</td><td>Branch Address</td><td>Account Type</td><td>Account Number</td></tr>";






                    }
                    else
                    {

                        bool segfordp = false;
                        segfordp = (HiddenField_SegmentName.Value.ToString().Contains("SPOT") || HiddenField_SegmentName.Value.ToString().Contains("COMM"));
                        if (dpcm.Rows.Count > 0 && !segfordp)
                        //if (dpcm.Rows.Count > 0 && segfordp)
                        {

                            for (int y = 0; y <= dpcm.Rows.Count - 1; y++)
                            {
                                dispTbl = dispTbl + "<tr  style=\"background-color: #ffffff;\"><td>&nbsp;" + dpcm.Rows[y]["AccountType"].ToString() + "</td><td>" + dpcm.Rows[y]["dipositoryID"].ToString() + "</td><td>" + dpcm.Rows[y]["DipositoryName"].ToString() + "</td><td>" + dpcm.Rows[y]["BenOwnerAccNo"].ToString() + "</td><td>&nbsp;" + dpcm.Rows[y]["IsPOA"].ToString() + "</td></tr>";
                            }
                            //dispTbl = dispTbl + "</table>";
                            dispTbl = dispTbl + "</td>";


                            // dispTbl = dispTbl + "</table>";
                            dispTbl = dispTbl + "</td></tr>";





                            //dispTbl = dispTbl + "<tr style=\"background-color: #ffffff;\" ><td align=\"left\"  valign=\"top\" colspan=\"2\">";
                            //dispTbl = dispTbl + "<table  cellpadding=\"1\" cellspacing=\"1\"  style=\"background-color: #ffffff; font-family: verdana; font-size: 12px;color:White;\"><tr style=\"background-color: #ffffff;\">";
                            //dispTbl = dispTbl + "<td style=\"padding:5px 6px 5px 6px\">";
                            //dispTbl = dispTbl + "<table  cellpadding=\"1\" cellspacing=\"1\" style=\"background-color: #99ccff; font-family: verdana; font-size: 12px;color:White;\"><tr><td colspan=\"6\" align=\"left\"><b>Bank Details</td></tr><tr  style=\"background-color: #ccffff;\"><td>Category</td><td>Bank Name</td><td>MICR No.</td><td>IFSC Code</td><td>Branch Address</td><td>Account Type</td><td>Account Number</td></tr>";



                        }
                        if (dpcomm.Rows.Count > 0 && segfordp)
                        {

                            for (int z = 0; z <= dpcomm.Rows.Count - 1; z++)
                            {
                                dispTbl = dispTbl + "<tr  style=\"background-color: #ffffff;\"><td>&nbsp;" + dpcomm.Rows[z]["AccountType"].ToString() + "</td><td>" + dpcomm.Rows[z]["dipositoryID"].ToString() + "</td><td>" + dpcomm.Rows[z]["DipositoryName"].ToString() + "</td><td>" + dpcomm.Rows[z]["BenOwnerAccNo"].ToString() + "</td><td>&nbsp;" + dpcomm.Rows[z]["IsPOA"].ToString() + "</td></tr>";
                            }
                            //dispTbl = dispTbl + "</table>";
                            dispTbl = dispTbl + "</td>";


                            //dispTbl = dispTbl + "</table>";
                            dispTbl = dispTbl + "</td></tr>";







                        }


                    }

                }
                else
                {
                    dispTbl = dispTbl + "<td style=\"padding:5px 6px 5px 6px\" valign=\"top\">";
                    dispTbl = dispTbl + "<table  cellpadding=\"1\" cellspacing=\"1\"  style=\"background-color: #99ccff; font-family: verdana; font-size: 12px;color:White;\"><tr><td colspan=\"3\" align=\"left\"><b>DP Details</td></tr><tr  style=\"background-color: #ccffff;\"><td>Category</td><td>Dipository ID</td><td>Name </td><td>Account ID</td><td>&nbsp;Is POA</td></tr>";
                    for (int m = 0; m <= dp.Rows.Count - 1; m++)
                    {
                        dispTbl = dispTbl + "<tr  style=\"background-color: #ffffff;\"><td>&nbsp;" + dp.Rows[m]["AccountType"].ToString() + "</td><td>" + dp.Rows[m]["dipositoryID"].ToString() + "</td><td>" + dp.Rows[m]["DipositoryName"].ToString() + "</td><td>" + dp.Rows[m]["BenOwnerAccNo"].ToString() + "</td><td>&nbsp;" + dp.Rows[m]["IsPOA"].ToString() + "</td></tr>";
                    }
                    //dispTbl = dispTbl + "</table>";
                    dispTbl = dispTbl + "</td>";


                    // dispTbl = dispTbl + "</table>";
                    dispTbl = dispTbl + "</td></tr>";





                    //dispTbl = dispTbl + "<tr style=\"background-color: #ffffff;\" ><td align=\"left\"  valign=\"top\" colspan=\"2\">";
                    //dispTbl = dispTbl + "<table  cellpadding=\"1\" cellspacing=\"1\"  style=\"background-color: #ffffff; font-family: verdana; font-size: 12px;color:White;\"><tr style=\"background-color: #ffffff;\">";
                    //dispTbl = dispTbl + "<td style=\"padding:5px 6px 5px 6px\">";
                    //dispTbl = dispTbl + "<table  cellpadding=\"1\" cellspacing=\"1\" style=\"background-color: #99ccff; font-family: verdana; font-size: 12px;color:White;\"><tr><td colspan=\"6\" align=\"left\"><b>Bank Details</td></tr><tr  style=\"background-color: #ccffff;\"><td>Category</td><td>Bank Name</td><td>MICR No.</td><td>IFSC Code</td><td>Branch Address</td><td>Account Type</td><td>Account Number</td></tr>";




                }
                dispTbl = dispTbl + "</table>";
                dispTbl = dispTbl + "</td>";


                dispTbl = dispTbl + "</table>";
                dispTbl = dispTbl + "</td></tr>";


                dispTbl = dispTbl + "<tr style=\"background-color: #ffffff;\" ><td align=\"left\"  valign=\"top\" colspan=\"2\">";
                dispTbl = dispTbl + "<table  cellpadding=\"1\" cellspacing=\"1\"  style=\"background-color: #ffffff; font-family: verdana; font-size: 12px;color:White;\"><tr style=\"background-color: #ffffff;\">";
                dispTbl = dispTbl + "<td style=\"padding:5px 6px 5px 6px\">";
                dispTbl = dispTbl + "<table  cellpadding=\"1\" cellspacing=\"1\" style=\"background-color: #99ccff; font-family: verdana; font-size: 12px;color:White;\"><tr><td colspan=\"6\" align=\"left\"><b>Bank Details</td></tr><tr  style=\"background-color: #ccffff;\"><td>Category</td><td>Bank Name</td><td>MICR No.</td><td>IFSC Code</td><td>Branch Address</td><td>Account Type</td><td>Account Number</td></tr>";

                for (int l = 0; l <= dtBank.Rows.Count - 1; l++)
                {
                    dispTbl = dispTbl + "<tr  style=\"background-color: #ffffff;\"><td>&nbsp;" + dtBank.Rows[l]["Category"].ToString() + "</td><td>" + dtBank.Rows[l]["BankName"].ToString() + "</td><td>&nbsp;" + dtBank.Rows[l]["BankMICR"].ToString() + "</td><td>&nbsp;" + dtBank.Rows[l]["BankIFSC"].ToString() + "</td><td>" + dtBank.Rows[l]["BranchAdress"].ToString() + "</td><td>" + dtBank.Rows[l]["AccountType"].ToString() + "</td><td>" + dtBank.Rows[l]["AccountNumber"].ToString() + "</td></tr>";
                }
                dispTbl = dispTbl + "</table>";
                dispTbl = dispTbl + "</td>";



                dispTbl = dispTbl + "</table>";
                dispTbl = dispTbl + "</td></tr>";

                DataTable dtBRMain = new DataTable();
                DataTable dtBRDetail = new DataTable();
                DataTable dtCharge = new DataTable();
                DataTable dtCHGRUP = new DataTable();



                if (rdbSegAll.Checked)
                {

                    string[] seg = HiddenField_Segment.Value.ToString().Trim().Split(',');
                    int r = seg.Length;
                    for (int t = 0; t < dtSeg.Rows.Count; t++)
                    {
                        DataTable dtSegid = oDBEngine.GetDataTable("(select exch_internalid  , (select rtrim(ltrim(exh_shortName)) from  tbl_master_Exchange where exh_cntId=exch_exchid)+' '+ '-' +' '+exch_segmentId  as SegmentName from  tbl_master_companyExchange ) as T ", " *  ", " T.SegmentName like '%" + dtSeg.Rows[t]["crg_exchange"].ToString().Trim() + "%' ");
                        DataTable dtsegName = oDBEngine.GetDataTable("tbl_master_companyExchange", "(select rtrim(ltrim(exh_shortName)) from  tbl_master_Exchange where exh_cntId=exch_exchid)+' '+ '-' +' '+exch_segmentId  as SegmentName ", "exch_internalid='" + dtSegid.Rows[0]["exch_internalid"].ToString().Trim() + "'");

                        DataTable dtCH = oDBEngine.GetDataTable("trans_chargegroupmembers", "ChargeGroupMembers_ID,ChargeGroupMembers_CustomerID,ChargeGroupMembers_CompanyID,ChargeGroupMembers_SegmentID,ChargeGroupMembers_GroupType,ChargeGroupMembers_GroupCode,CONVERT(VARCHAR(10), ChargeGroupMembers_FromDate, 120) as ChargeGroupMembers_FromDate,CONVERT(VARCHAR(10), ChargeGroupMembers_UntilDate, 120) as ChargeGroupMembers_UntilDate ", "ChargeGroupMembers_CustomerId='" + DtMain.Rows[PageNo]["cnt_internalid"].ToString().Trim() + "' and ChargeGroupMembers_SegmentID = '" + dtSegid.Rows[0]["exch_internalid"].ToString().Trim() + "' and ChargeGroupMembers_UntilDate is null");

                        if (dtCH.Rows.Count > 0)
                        {

                            if (dtCH.Rows[0]["ChargeGroupMembers_GroupType"].ToString().Trim() == "1")
                            {


                                dtBRMain = oDBEngine.GetDataTable("Config_BrokerageMain", "BrokerageMain_ID,BrokerageMain_CustomerID,(select cmp_Name from tbl_master_company where cmp_internalid=BrokerageMain_CompanyID) as BrokerageMain_CompanyID,BrokerageMain_SegmentID,BrokerageMain_BrkgDecimals,case when BrokerageMain_BrkgRoundPattern=1 then 'Nearest' when BrokerageMain_BrkgRoundPattern=2 then 'Higher' when BrokerageMain_BrkgRoundPattern=3 then 'Lower' when BrokerageMain_BrkgRoundPattern=4 then 'Truncate ' when BrokerageMain_BrkgRoundPattern=5 then 'Nearest 5 Paisa' when BrokerageMain_BrkgRoundPattern=6 then 'Lower 5 Paisa' when BrokerageMain_BrkgRoundPattern=7 then 'Higher 5 Paisa' end as BrokerageMain_BrkgRoundPattern,case when BrokerageMain_AverageType=1 then 'None' when BrokerageMain_AverageType=2 then 'Order Number' when BrokerageMain_AverageType=3 then 'Instrument' when BrokerageMain_AverageType=4 then 'Similar Price' else 'None' end as BrokerageMain_AverageType ,BrokerageMain_MktDecimals,case when BrokerageMain_MktRoundPattern=1 then 'Nearest' when BrokerageMain_MktRoundPattern=2 then 'Higher' when BrokerageMain_MktRoundPattern=3 then 'Lower' when BrokerageMain_MktRoundPattern=4 then 'Truncate ' when BrokerageMain_MktRoundPattern=5 then 'Nearest 5 Paisa' when BrokerageMain_MktRoundPattern=6 then 'Lower 5 Paisa' when BrokerageMain_MktRoundPattern=7 then 'Higher 5 Paisa' end as BrokerageMain_MktRoundPattern,BrokerageMain_NetDecimals,case when BrokerageMain_NetRoundPattern=1 then 'None' when BrokerageMain_NetRoundPattern=1 then 'None' when BrokerageMain_NetRoundPattern=2 then 'Nearest Rupee' when BrokerageMain_NetRoundPattern=3 then 'Higher Rupee' when BrokerageMain_NetRoundPattern=4 then 'Lower Rupee' else 'None' end as BrokerageMain_NetRoundPattern, case  when BrokerageMain_ContractPattern=1 then 'Single' when BrokerageMain_ContractPattern=2 then 'Sharewise' when BrokerageMain_ContractPattern=3 then 'Orderwise' when BrokerageMain_ContractPattern=4 then 'Order+Sharewise' when BrokerageMain_ContractPattern=5 then 'None' else 'None' end as BrokerageMain_ContractPattern, BrokerageMain_MinDelPerShare, BrokerageMain_MaxDelPerShare, BrokerageMain_MinSqrPerShare, BrokerageMain_MaxSqrPerShare, BrokerageMain_MinDailyBrkg, BrokerageMain_MaxDailyBrkg,  BrokerageMain_MinBrkgPerOrder,BrokerageMain_MaxBrkgPerOrder, CONVERT(VARCHAR(11), BrokerageMain_FromDate , 106) as BrokerageMain_FromDate,BrokerageMain_Type  ", "BrokerageMain_CustomerID='" + DtMain.Rows[PageNo]["cnt_internalid"].ToString().Trim() + "' and  BrokerageMain_FromDate <= '" + dtCH.Rows[0]["ChargeGroupMembers_FromDate"].ToString().Trim() + "' and (BrokerageMain_UntilDate is  null or BrokerageMain_UntilDate >= '" + dtCH.Rows[0]["ChargeGroupMembers_FromDate"].ToString().Trim() + "') and BrokerageMain_SegmentID='" + dtSegid.Rows[0]["exch_internalid"].ToString().Trim() + "'");
                                if (dtBRMain.Rows.Count > 0)
                                {
                                    dtCharge = oDBEngine.GetDataTable("Config_ChargeSetup", "case when  ChargeSetup_ChargeType='CL' then 'Clearing Charges' when  ChargeSetup_ChargeType='SD' then 'Stamp Duty' when  ChargeSetup_ChargeType='DM' then 'Demat Charges' when  ChargeSetup_ChargeType='SF' then 'SEBI Fee' when  ChargeSetup_ChargeType='SX' then 'STT' when  ChargeSetup_ChargeType='ST' then 'Service Tax' when  ChargeSetup_ChargeType='TX' then 'Transaction Charge' end as ChargeSetup_ChargeType , case when ChargeSetup_ChargeBasis=1 then 'Inclusive' when ChargeSetup_ChargeBasis=2 then 'Exclusive' when ChargeSetup_ChargeBasis=3 then 'Not Applicable' end  as ChargeSetup_ChargeBasis  , (Select (isnull(ChargeGroup_Name,''))+'[' + ChargeGroup_Code+']'as d from Master_ChargeGroup WHERE  ChargeGroup_Type='2' and ChargeGroup_Code= ChargeSetup_ChargeGroup ) as  ChargeSetup_ChargeGroup ", " ChargeSetup_MainID='" + dtBRMain.Rows[0]["BrokerageMain_ID"].ToString().Trim() + "' ");
                                }
                            }
                            else if (dtCH.Rows[0]["ChargeGroupMembers_GroupType"].ToString().Trim() == "2")
                            {
                                dtCHGRUP = oDBEngine.GetDataTable("master_chargegroup", "*", "chargeGroup_code='" + dtCH.Rows[0]["ChargeGroupMembers_GroupCode"].ToString().Trim() + "' and chargeGroup_Type=1");
                                dtBRMain = oDBEngine.GetDataTable("Config_BrokerageMain", "BrokerageMain_ID,BrokerageMain_CustomerID,(select cmp_Name from tbl_master_company where cmp_internalid=BrokerageMain_CompanyID) as BrokerageMain_CompanyID,BrokerageMain_SegmentID,BrokerageMain_BrkgDecimals,case when BrokerageMain_BrkgRoundPattern=1 then 'Nearest' when BrokerageMain_BrkgRoundPattern=2 then 'Higher' when BrokerageMain_BrkgRoundPattern=3 then 'Lower' when BrokerageMain_BrkgRoundPattern=4 then 'Truncate ' when BrokerageMain_BrkgRoundPattern=5 then 'Nearest 5 Paisa' when BrokerageMain_BrkgRoundPattern=6 then 'Lower 5 Paisa' when BrokerageMain_BrkgRoundPattern=7 then 'Higher 5 Paisa' end as BrokerageMain_BrkgRoundPattern,case when BrokerageMain_AverageType=1 then 'None' when BrokerageMain_AverageType=2 then 'Order Number' when BrokerageMain_AverageType=3 then 'Instrument' when BrokerageMain_AverageType=4 then 'Similar Price' else 'None' end as BrokerageMain_AverageType ,BrokerageMain_MktDecimals,case when BrokerageMain_MktRoundPattern=1 then 'Nearest' when BrokerageMain_MktRoundPattern=2 then 'Higher' when BrokerageMain_MktRoundPattern=3 then 'Lower' when BrokerageMain_MktRoundPattern=4 then 'Truncate ' when BrokerageMain_MktRoundPattern=5 then 'Nearest 5 Paisa' when BrokerageMain_MktRoundPattern=6 then 'Lower 5 Paisa' when BrokerageMain_MktRoundPattern=7 then 'Higher 5 Paisa' end as BrokerageMain_MktRoundPattern,BrokerageMain_NetDecimals,case when BrokerageMain_NetRoundPattern=1 then 'None' when BrokerageMain_NetRoundPattern=1 then 'None' when BrokerageMain_NetRoundPattern=2 then 'Nearest Rupee' when BrokerageMain_NetRoundPattern=3 then 'Higher Rupee' when BrokerageMain_NetRoundPattern=4 then 'Lower Rupee' else 'None' end as BrokerageMain_NetRoundPattern, case  when BrokerageMain_ContractPattern=1 then 'Single' when BrokerageMain_ContractPattern=2 then 'Sharewise' when BrokerageMain_ContractPattern=3 then 'Orderwise' when BrokerageMain_ContractPattern=4 then 'Order+Sharewise' when BrokerageMain_ContractPattern=5 then 'None' else 'None' end as BrokerageMain_ContractPattern, BrokerageMain_MinDelPerShare, BrokerageMain_MaxDelPerShare, BrokerageMain_MinSqrPerShare, BrokerageMain_MaxSqrPerShare, BrokerageMain_MinDailyBrkg, BrokerageMain_MaxDailyBrkg,  BrokerageMain_MinBrkgPerOrder,BrokerageMain_MaxBrkgPerOrder,CONVERT(VARCHAR(11), BrokerageMain_FromDate , 106) as BrokerageMain_FromDate,BrokerageMain_Type   ", "BrokerageMain_CustomerID='" + dtCHGRUP.Rows[0]["ChargeGroup_Code"].ToString().Trim() + "' and  BrokerageMain_FromDate <= '" + dtCH.Rows[0]["ChargeGroupMembers_FromDate"].ToString().Trim() + "' and (BrokerageMain_UntilDate is  null or BrokerageMain_UntilDate >= '" + dtCH.Rows[0]["ChargeGroupMembers_FromDate"].ToString().Trim() + "') and BrokerageMain_SegmentID='" + dtSegid.Rows[0]["exch_internalid"].ToString().Trim() + "'");

                                if (dtBRMain.Rows.Count > 0)
                                {
                                    dtCharge = oDBEngine.GetDataTable("Config_ChargeSetup", "case when  ChargeSetup_ChargeType='CL' then 'Clearing Charges' when  ChargeSetup_ChargeType='SD' then 'Stamp Duty' when  ChargeSetup_ChargeType='DM' then 'Demat Charges' when  ChargeSetup_ChargeType='SF' then 'SEBI Fee' when  ChargeSetup_ChargeType='SX' then 'STT' when  ChargeSetup_ChargeType='ST' then 'Service Tax' when  ChargeSetup_ChargeType='TX' then 'Transaction Charge' end as ChargeSetup_ChargeType , case when ChargeSetup_ChargeBasis=1 then 'Inclusive' when ChargeSetup_ChargeBasis=2 then 'Exclusive' when ChargeSetup_ChargeBasis=3 then 'Not Applicable' end  as ChargeSetup_ChargeBasis  , (Select (isnull(ChargeGroup_Name,''))+'[' + ChargeGroup_Code+']'as d from Master_ChargeGroup WHERE  ChargeGroup_Type='2' and ChargeGroup_Code=ChargeSetup_ChargeGroup) as  ChargeSetup_ChargeGroup ", " ChargeSetup_MainID='" + dtBRMain.Rows[0]["BrokerageMain_ID"].ToString().Trim() + "' ");
                                }

                            }

                        }

                        if (dtBRMain.Rows.Count > 0)
                        {
                            dtBRDetail = oDBEngine.GetDataTable("Config_BrokerageDetail as cbd ,Config_BrokerageMain as cbm ", "cbd.BrokerageDetail_ProductID,case when cbd.BrokerageDetail_MktSegment=1 then 'All' when  cbd.BrokerageDetail_MktSegment=2 then 'Rolling' when  cbd.BrokerageDetail_MktSegment=3 then 'T2T' when  cbd.BrokerageDetail_MktSegment=4 then 'Physical'  when  cbd.BrokerageDetail_MktSegment=5 then 'Institutional' when cbd.BrokerageDetail_MktSegment=6 then 'Auction' else ' ' end as BrokerageDetail_MktSegment,cbd.BrokerageDetail_SlabCode,cbd.BrokerageDetail_ID,cast(cbd.BrokerageDetail_FlatRate  as decimal(18,2))as BrokerageDetail_FlatRate,cbd.BrokerageDetail_Rate,cast(cbd.BrokerageDetail_MinAmount  as decimal(18,2)) as BrokerageDetail_MinAmount,case when cbd.BrokerageDetail_BrkgFor=1 then 'All'  else (select Products_Name from Master_Products where Products_ID=cbd.BrokerageDetail_ProductID) end as BrokerageDetail_BrkgFor,case when cbd.BrokerageDetail_BrkgType=1 then 'Delivery' when cbd.BrokerageDetail_BrkgType=2 then 'Square-Off' when cbd.BrokerageDetail_BrkgType=3 then 'Exercise' when cbd.BrokerageDetail_BrkgType=4 then 'Assignment' when cbd.BrokerageDetail_BrkgType=6 then 'Delivery CloseValue' else 'Final Settlement' end as BrokerageDetail_BrkgType,case when  cbd.BrokerageDetail_TranType=1 then 'Purchase' when cbd.BrokerageDetail_TranType=2 then 'Sale' when cbd.BrokerageDetail_TranType=3 then 'Both' when cbd.BrokerageDetail_TranType=4 then 'FirstLeg' when cbd.BrokerageDetail_TranType=5 then 'SecondLeg' when cbd.BrokerageDetail_TranType=6 then 'HigherLeg' when cbd.BrokerageDetail_TranType=7  then 'LowerLeg' when cbd.BrokerageDetail_TranType=8 then 'Daily' when cbd.BrokerageDetail_TranType=9 then 'DailySecond' when cbd.BrokerageDetail_TranType=10 then 'Carry' else 'CarrySecond' end as BrokerageDetail_TranType,case when cbd.BrokerageDetail_InstrType=1 then 'All' when cbd.BrokerageDetail_InstrType=2 then 'Equity' when cbd.BrokerageDetail_InstrType=3 then 'Bonds'  when cbd.BrokerageDetail_InstrType=4 then 'Debt' when cbd.BrokerageDetail_InstrType=5 then 'ETFs' when cbd.BrokerageDetail_InstrType=6 then ' Equity Futures' when cbd.BrokerageDetail_InstrType=7 then 'Equity Options'  when cbd.BrokerageDetail_InstrType=8 then 'Index Futures'when cbd.BrokerageDetail_InstrType=9 then 'Index Options'when cbd.BrokerageDetail_InstrType=10 then 'All Futures'when cbd.BrokerageDetail_InstrType=11 then 'All Options' when cbd.BrokerageDetail_InstrType=12 then 'Comm Futures'when cbd.BrokerageDetail_InstrType=13 then 'Comm Options'  when cbd.BrokerageDetail_InstrType=14 then 'All Futures' else 'All Options' end as BrokerageDetail_InstrType ", "cbd.BrokerageDetail_MainID='" + dtBRMain.Rows[0]["BrokerageMain_ID"].ToString() + "' and cbd.BrokerageDetail_MainID=cbm.BrokerageMain_ID");
                        }

                        if (dtBRMain.Rows.Count > 0 && dtBRDetail.Rows.Count > 0 && dtCharge.Rows.Count > 0)
                        {
                            dispTbl = dispTbl + "<tr  style=\"background-color: #99ccff;\"><td   colspan=\"2\" style=\"padding:5px 6px 5px 6px\"><b>Brokerage Setup in " + dtsegName.Rows[0]["SegmentName"].ToString();
                            if (dtCH.Rows[0]["ChargeGroupMembers_GroupType"].ToString().Trim() == "1")
                            {
                                dispTbl = dispTbl + "&nbsp;&nbsp;&nbsp; Type: <b>Specific </td></tr>";
                            }
                            else if (dtCH.Rows[0]["ChargeGroupMembers_GroupType"].ToString().Trim() == "2")
                            {

                                dispTbl = dispTbl + "&nbsp;&nbsp;&nbsp; Type: <b>Scheme [Group Name:" + dtCHGRUP.Rows[0]["ChargeGroup_Name"].ToString() + "[" + dtCHGRUP.Rows[0]["ChargeGroup_Code"].ToString() + "]]</td></tr>";
                            }

                        }

                        if (dtBRMain.Rows.Count > 0)
                        {

                            dispTbl = dispTbl + "<tr style=\"background-color: #ffffff;\" ><td align=\"left\"  valign=\"top\" colspan=\"2\" style=\"padding:5px 6px 5px 6px\">";

                            dispTbl = dispTbl + "<table  cellpadding=\"1\" cellspacing=\"1\"  style=\"background-color: #ffffff; font-family: verdana; font-size: 12px;color:White;\"><tr style=\"background-color: #ffffff;\">";
                            dispTbl = dispTbl + "<td valign=\"top\">";

                            dispTbl = dispTbl + "<table  cellpadding=\"1\" cellspacing=\"1\"  style=\"background-color: #99ccff; font-family: verdana; font-size: 12px;color:White;\">";
                            dispTbl = dispTbl + "<tr ><td align=\"left\"  valign=\"top\" colspan=\"8\"><b>Brokerage[General] &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<b>Company:" + dtBRMain.Rows[0]["BrokerageMain_CompanyID"].ToString().Trim() + "&nbsp;&nbsp;&nbsp;&nbsp;<b>Date:" + dtBRMain.Rows[0]["BrokerageMain_FromDate"].ToString().Trim() + "</td></tr>";
                            dispTbl = dispTbl + "<tr  style=\"background-color: #ffffff;\"><td align=\"left\"  valign=\"top\">Brkg Decimals:</td><td align=\"left\"  valign=\"top\">" + dtBRMain.Rows[0]["BrokerageMain_BrkgDecimals"].ToString().Trim() + "</td><td align=\"left\"  valign=\"top\">Brkg.Rd-Off:</td><td align=\"left\"  valign=\"top\">" + dtBRMain.Rows[0]["BrokerageMain_BrkgRoundPattern"].ToString().Trim() + "</td><td align=\"left\"  valign=\"top\">Min Daily Brkg:</td><td align=\"left\"  valign=\"top\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dtBRMain.Rows[0]["BrokerageMain_MinDailyBrkg"].ToString().Trim())) + "</td><td align=\"left\"  valign=\"top\">Min Sqr Brkg/Shr:</td><td align=\"left\"  valign=\"top\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dtBRMain.Rows[0]["BrokerageMain_MinSqrPerShare"].ToString().Trim())) + "</td></tr>";
                            dispTbl = dispTbl + "<tr  style=\"background-color: #ffffff;\"><td align=\"left\"  valign=\"top\">Net Amt Decimal:</td><td align=\"left\"  valign=\"top\">" + dtBRMain.Rows[0]["BrokerageMain_NetDecimals"].ToString().Trim() + "</td><td align=\"left\"  valign=\"top\">Mkt. Rd-Off:</td><td align=\"left\"  valign=\"top\">" + dtBRMain.Rows[0]["BrokerageMain_MktRoundPattern"].ToString().Trim() + "</td><td align=\"left\"  valign=\"top\">Max Daily Brkg:</td><td align=\"left\"  valign=\"top\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dtBRMain.Rows[0]["BrokerageMain_MaxDailyBrkg"].ToString().Trim())) + "</td><td align=\"left\"  valign=\"top\">Max Sqr Brkg/Shr:</td><td align=\"left\"  valign=\"top\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dtBRMain.Rows[0]["BrokerageMain_MaxSqrPerShare"].ToString().Trim())) + "</td></tr>";
                            dispTbl = dispTbl + "<tr  style=\"background-color: #ffffff;\"><td align=\"left\"  valign=\"top\">Mkt Rate Decimal:</td><td align=\"left\"  valign=\"top\">" + dtBRMain.Rows[0]["BrokerageMain_MktDecimals"].ToString().Trim() + "</td><td align=\"left\"  valign=\"top\">Net Rd-Off:</td><td align=\"left\"  valign=\"top\">" + dtBRMain.Rows[0]["BrokerageMain_NetRoundPattern"].ToString().Trim() + "</td><td align=\"left\"  valign=\"top\">Min Del Brkg/Shr:</td><td align=\"left\"  valign=\"top\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dtBRMain.Rows[0]["BrokerageMain_MinDelPerShare"].ToString().Trim())) + "</td><td align=\"left\"  valign=\"top\">Min Brkg/Order:</td><td align=\"left\"  valign=\"top\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dtBRMain.Rows[0]["BrokerageMain_MinBrkgPerOrder"].ToString().Trim())) + "</td></tr>";
                            dispTbl = dispTbl + "<tr  style=\"background-color: #ffffff;\"><td align=\"left\"  valign=\"top\">Trd Avg.Type:</td><td align=\"left\"  valign=\"top\">" + dtBRMain.Rows[0]["BrokerageMain_AverageType"].ToString().Trim() + "</td><td align=\"left\"  valign=\"top\">Cont. Pattern:</td><td align=\"left\"  valign=\"top\">" + dtBRMain.Rows[0]["BrokerageMain_ContractPattern"].ToString().Trim() + "</td><td align=\"left\"  valign=\"top\">Max Del Brkg/Shr:</td><td align=\"left\"  valign=\"top\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dtBRMain.Rows[0]["BrokerageMain_MaxDelPerShare"].ToString().Trim())) + "</td><td align=\"left\"  valign=\"top\">Max Brkg/Order:</td><td align=\"left\"  valign=\"top\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dtBRMain.Rows[0]["BrokerageMain_MaxBrkgPerOrder"].ToString().Trim())) + "</td></tr>";
                            dispTbl = dispTbl + "</table>";

                            dispTbl = dispTbl + "</td>";
                            dispTbl = dispTbl + "<td style=\"padding:0px 6px 0px 6px\" valign=\"top\">";

                            if (dtCharge.Rows.Count > 0)
                            {
                                dispTbl = dispTbl + "<table  cellpadding=\"1\" cellspacing=\"1\" style=\"background-color: #99ccff; font-family: verdana; font-size: 12px;color:White;\"><tr align=\"left\" ><td align=\"left\"  valign=\"top\" colspan=\"3\"><b>Charge Setup[Details]</td></tr><tr  style=\"background-color: #ccffff;\"><td>&nbsp;</td><td>Scheme</td><td>Basis</td></tr>";
                                for (int p = 0; p <= dtCharge.Rows.Count - 1; p++)
                                {
                                    dispTbl = dispTbl + "<tr  style=\"background-color: #ffffff;\"><td align=\"left\">" + dtCharge.Rows[p]["ChargeSetup_ChargeType"].ToString() + "</td><td align=\"left\">" + dtCharge.Rows[p]["ChargeSetup_ChargeGroup"].ToString() + "</td><td align=\"left\">" + dtCharge.Rows[p]["ChargeSetup_ChargeBasis"].ToString() + "</td></tr>";
                                }
                                dispTbl = dispTbl + "</table>";


                            }

                            dispTbl = dispTbl + "</td>";
                            dispTbl = dispTbl + "</table>";



                            dispTbl = dispTbl + "</td></tr>";
                        }
                        if (dtBRDetail.Rows.Count > 0)
                        {
                            dispTbl = dispTbl + "<tr style=\"background-color: #ffffff;\"  align=\"left\"><td align=\"left\" colspan=\"2\"  valign=\"top\"  style=\"padding:5px 6px 5px 6px\">";
                            dispTbl = dispTbl + "<table  cellpadding=\"1\" cellspacing=\"1\" style=\"background-color: #99ccff; font-family: verdana; font-size: 12px;color:White;\"><tr align=\"left\" ><td align=\"left\"  valign=\"top\" colspan=\"9\"><b>Brokerage Setup[Details]:</td></tr><tr  style=\"background-color: #ccffff;\"><td>Market Segment</td><td>Brokerage Type</td><td>Brokerage For</td><td>Transaction Type</td><td>Instrument Type</td><td>Flat Amount</td><td>Rate</td><td>Min. Amount</td><td>Brokerage Slab</td></tr>";
                            for (int n = 0; n <= dtBRDetail.Rows.Count - 1; n++)
                            {
                                dispTbl = dispTbl + "<tr  style=\"background-color: #ffffff;\"><td align=\"left\">" + dtBRDetail.Rows[n]["BrokerageDetail_MktSegment"].ToString() + "</td><td align=\"left\">" + dtBRDetail.Rows[n]["BrokerageDetail_BrkgType"].ToString() + "</td><td align=\"left\">" + dtBRDetail.Rows[n]["BrokerageDetail_BrkgFor"].ToString() + "</td><td align=\"left\">" + dtBRDetail.Rows[n]["BrokerageDetail_TranType"].ToString() + "</td><td align=\"left\">" + dtBRDetail.Rows[n]["BrokerageDetail_InstrType"].ToString() + "</td><td align=\"left\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dtBRDetail.Rows[n]["BrokerageDetail_FlatRate"].ToString())) + "</td><td align=\"left\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dtBRDetail.Rows[n]["BrokerageDetail_Rate"].ToString())) + "</td><td align=\"left\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dtBRDetail.Rows[n]["BrokerageDetail_MinAmount"].ToString())) + "</td><td align=\"left\">" + dtBRDetail.Rows[n]["BrokerageDetail_SlabCode"].ToString() + "</td></tr>";
                            }
                            dispTbl = dispTbl + "</table>";
                            dispTbl = dispTbl + "</td></tr>";

                        }
                        if (dtCPD.Rows.Count > 0)
                        {
                            dispTbl = dispTbl + "<tr style=\"background-color: #ffffff;\"  align=\"left\"><td align=\"left\" colspan=\"2\"  valign=\"top\"  style=\"padding:5px 6px 5px 6px\">";
                            dispTbl = dispTbl + "<table  cellpadding=\"1\" cellspacing=\"1\" style=\"background-color: #99ccff; font-family: verdana; font-size: 12px;color:White;\"><tr align=\"left\" ><td align=\"left\"  valign=\"top\" colspan=\"9\"><b>Contact Person Details:</td></tr><tr  style=\"background-color: #ccffff;\"><td>Name</td><td>Relationship</td><td>Designation</td><td>Phone</td><td>Email</td><td>Status</td><td>PAN Number</td><td>Din</td></tr>";

                            for (int rows = 0; rows < dtCPD.Rows.Count; rows++)
                            {
                                dispTbl = dispTbl + "<tr  style=\"background-color: #ffffff;\"><td align=\"left\">" + Convert.ToString(dtCPD.Rows[rows]["name"]) + "</td>" +
                                    "<td align=\"left\">" + Convert.ToString(dtCPD.Rows[rows]["cp_relationShip"]) + "</td>" +
                                    "<td align=\"left\">" + Convert.ToString(dtCPD.Rows[rows]["cp_designation"]) + "</td>" +
                                    "<td align=\"left\">" + Convert.ToString(dtCPD.Rows[rows]["Phone"]) + "</td>" +
                                    "<td align=\"left\">" + Convert.ToString(dtCPD.Rows[rows]["email"]) + "</td>" +
                                    "<td align=\"left\">" + Convert.ToString(dtCPD.Rows[rows]["cp_status"]) + "</td>" +
                                    "<td align=\"left\">" + Convert.ToString(dtCPD.Rows[rows]["cp_Pan"]) + "</td>" +
                                    "<td align=\"left\">" + Convert.ToString(dtCPD.Rows[rows]["cp_Din"]) + "</td></tr>";

                            }
                            dispTbl = dispTbl + "</table>";
                            dispTbl = dispTbl + "</td></tr>";

                        }

                        dtCH.Clear();
                        dtCharge.Clear();
                        dtBRMain.Clear();
                        dtBRDetail.Clear();
                        dtCHGRUP.Clear();
                        dtCPD.Clear();


                    }


                }
                else
                {


                    string[] seg = HiddenField_Segment.Value.ToString().Trim().Split(',');
                    int r = seg.Length;
                    for (int s = 0; s < r; s++)
                    {
                        DataTable dtsegName = oDBEngine.GetDataTable("tbl_master_companyExchange", "(select rtrim(ltrim(exh_shortName)) from  tbl_master_Exchange where exh_cntId=exch_exchid)+' '+ '-' +' '+exch_segmentId  as SegmentName ", "exch_internalid='" + seg[s].ToString().Trim() + "'");
                        //  dispTbl = dispTbl + "<tr  style=\"background-color: #99ccff;\"><td  colspan=\"2\" style=\"padding:5px 6px 5px 6px\"><b>Brokerage Setup in " + dtsegName.Rows[0]["SegmentName"].ToString();

                        DataTable dtCH = oDBEngine.GetDataTable("trans_chargegroupmembers", "ChargeGroupMembers_ID,ChargeGroupMembers_CustomerID,ChargeGroupMembers_CompanyID,ChargeGroupMembers_SegmentID,ChargeGroupMembers_GroupType,ChargeGroupMembers_GroupCode,CONVERT(VARCHAR(10), ChargeGroupMembers_FromDate, 120) as ChargeGroupMembers_FromDate,CONVERT(VARCHAR(10), ChargeGroupMembers_UntilDate, 120) as ChargeGroupMembers_UntilDate ", "ChargeGroupMembers_CustomerId='" + DtMain.Rows[PageNo]["cnt_internalid"].ToString().Trim() + "' and ChargeGroupMembers_SegmentID = '" + seg[s].ToString().Trim() + "'");

                        if (dtCH.Rows.Count > 0)
                        {

                            if (dtCH.Rows[0]["ChargeGroupMembers_GroupType"].ToString().Trim() == "1")
                            {
                                //  dispTbl = dispTbl + "&nbsp;&nbsp;&nbsp; Type: <b>Specific </td></tr>";

                                dtBRMain = oDBEngine.GetDataTable("Config_BrokerageMain", "BrokerageMain_ID,BrokerageMain_CustomerID,(select cmp_Name from tbl_master_company where cmp_internalid=BrokerageMain_CompanyID) as BrokerageMain_CompanyID,BrokerageMain_SegmentID,BrokerageMain_BrkgDecimals,case when BrokerageMain_BrkgRoundPattern=1 then 'Nearest' when BrokerageMain_BrkgRoundPattern=2 then 'Higher' when BrokerageMain_BrkgRoundPattern=3 then 'Lower' when BrokerageMain_BrkgRoundPattern=4 then 'Truncate ' when BrokerageMain_BrkgRoundPattern=5 then 'Nearest 5 Paisa' when BrokerageMain_BrkgRoundPattern=6 then 'Lower 5 Paisa' when BrokerageMain_BrkgRoundPattern=7 then 'Higher 5 Paisa' end as BrokerageMain_BrkgRoundPattern,case when BrokerageMain_AverageType=1 then 'None' when BrokerageMain_AverageType=2 then 'Order Number' when BrokerageMain_AverageType=3 then 'Instrument' when BrokerageMain_AverageType=4 then 'Similar Price' else 'None' end as BrokerageMain_AverageType ,BrokerageMain_MktDecimals,case when BrokerageMain_MktRoundPattern=1 then 'Nearest' when BrokerageMain_MktRoundPattern=2 then 'Higher' when BrokerageMain_MktRoundPattern=3 then 'Lower' when BrokerageMain_MktRoundPattern=4 then 'Truncate ' when BrokerageMain_MktRoundPattern=5 then 'Nearest 5 Paisa' when BrokerageMain_MktRoundPattern=6 then 'Lower 5 Paisa' when BrokerageMain_MktRoundPattern=7 then 'Higher 5 Paisa' end as BrokerageMain_MktRoundPattern,BrokerageMain_NetDecimals,case when BrokerageMain_NetRoundPattern=1 then 'None' when BrokerageMain_NetRoundPattern=1 then 'None' when BrokerageMain_NetRoundPattern=2 then 'Nearest Rupee' when BrokerageMain_NetRoundPattern=3 then 'Higher Rupee' when BrokerageMain_NetRoundPattern=4 then 'Lower Rupee' else 'None' end as BrokerageMain_NetRoundPattern, case  when BrokerageMain_ContractPattern=1 then 'Single' when BrokerageMain_ContractPattern=2 then 'Sharewise' when BrokerageMain_ContractPattern=3 then 'Orderwise' when BrokerageMain_ContractPattern=4 then 'Order+Sharewise' when BrokerageMain_ContractPattern=5 then 'None' else 'None' end as BrokerageMain_ContractPattern, BrokerageMain_MinDelPerShare, BrokerageMain_MaxDelPerShare, BrokerageMain_MinSqrPerShare, BrokerageMain_MaxSqrPerShare, BrokerageMain_MinDailyBrkg, BrokerageMain_MaxDailyBrkg,  BrokerageMain_MinBrkgPerOrder,BrokerageMain_MaxBrkgPerOrder, CONVERT(VARCHAR(11), BrokerageMain_FromDate , 106) as BrokerageMain_FromDate,BrokerageMain_Type  ", "BrokerageMain_CustomerID='" + DtMain.Rows[PageNo]["cnt_internalid"].ToString().Trim() + "' and  BrokerageMain_FromDate <= '" + dtCH.Rows[0]["ChargeGroupMembers_FromDate"].ToString().Trim() + "' and (BrokerageMain_UntilDate is  null or BrokerageMain_UntilDate >= '" + dtCH.Rows[0]["ChargeGroupMembers_FromDate"].ToString().Trim() + "') and BrokerageMain_SegmentID='" + seg[s].ToString().Trim() + "'");
                                if (dtBRMain.Rows.Count > 0)
                                {
                                    dtCharge = oDBEngine.GetDataTable("Config_ChargeSetup", "case when  ChargeSetup_ChargeType='CL' then 'Clearing Charges' when  ChargeSetup_ChargeType='SD' then 'Stamp Duty' when  ChargeSetup_ChargeType='DM' then 'Demat Charges' when  ChargeSetup_ChargeType='SF' then 'SEBI Fee' when  ChargeSetup_ChargeType='SX' then 'STT' when  ChargeSetup_ChargeType='ST' then 'Service Tax' when  ChargeSetup_ChargeType='TX' then 'Transaction Charge' end as ChargeSetup_ChargeType , case when ChargeSetup_ChargeBasis=1 then 'Inclusive' when ChargeSetup_ChargeBasis=2 then 'Exclusive' when ChargeSetup_ChargeBasis=3 then 'Not Applicable' end  as ChargeSetup_ChargeBasis  , (Select (isnull(ChargeGroup_Name,''))+'[' + ChargeGroup_Code+']'as d from Master_ChargeGroup WHERE  ChargeGroup_Type='2' and ChargeGroup_Code= ChargeSetup_ChargeGroup ) as  ChargeSetup_ChargeGroup ", " ChargeSetup_MainID='" + dtBRMain.Rows[0]["BrokerageMain_ID"].ToString().Trim() + "' ");
                                }
                            }
                            else if (dtCH.Rows[0]["ChargeGroupMembers_GroupType"].ToString().Trim() == "2")
                            {
                                dtCHGRUP = oDBEngine.GetDataTable("master_chargegroup", "*", "chargeGroup_code='" + dtCH.Rows[0]["ChargeGroupMembers_GroupCode"].ToString().Trim() + "' and chargeGroup_Type=1");
                                dtBRMain = oDBEngine.GetDataTable("Config_BrokerageMain", "BrokerageMain_ID,BrokerageMain_CustomerID,(select cmp_Name from tbl_master_company where cmp_internalid=BrokerageMain_CompanyID) as BrokerageMain_CompanyID,BrokerageMain_SegmentID,BrokerageMain_BrkgDecimals,case when BrokerageMain_BrkgRoundPattern=1 then 'Nearest' when BrokerageMain_BrkgRoundPattern=2 then 'Higher' when BrokerageMain_BrkgRoundPattern=3 then 'Lower' when BrokerageMain_BrkgRoundPattern=4 then 'Truncate ' when BrokerageMain_BrkgRoundPattern=5 then 'Nearest 5 Paisa' when BrokerageMain_BrkgRoundPattern=6 then 'Lower 5 Paisa' when BrokerageMain_BrkgRoundPattern=7 then 'Higher 5 Paisa' end as BrokerageMain_BrkgRoundPattern,case when BrokerageMain_AverageType=1 then 'None' when BrokerageMain_AverageType=2 then 'Order Number' when BrokerageMain_AverageType=3 then 'Instrument' when BrokerageMain_AverageType=4 then 'Similar Price' else 'None' end as BrokerageMain_AverageType ,BrokerageMain_MktDecimals,case when BrokerageMain_MktRoundPattern=1 then 'Nearest' when BrokerageMain_MktRoundPattern=2 then 'Higher' when BrokerageMain_MktRoundPattern=3 then 'Lower' when BrokerageMain_MktRoundPattern=4 then 'Truncate ' when BrokerageMain_MktRoundPattern=5 then 'Nearest 5 Paisa' when BrokerageMain_MktRoundPattern=6 then 'Lower 5 Paisa' when BrokerageMain_MktRoundPattern=7 then 'Higher 5 Paisa' end as BrokerageMain_MktRoundPattern,BrokerageMain_NetDecimals,case when BrokerageMain_NetRoundPattern=1 then 'None' when BrokerageMain_NetRoundPattern=1 then 'None' when BrokerageMain_NetRoundPattern=2 then 'Nearest Rupee' when BrokerageMain_NetRoundPattern=3 then 'Higher Rupee' when BrokerageMain_NetRoundPattern=4 then 'Lower Rupee' else 'None' end as BrokerageMain_NetRoundPattern, case  when BrokerageMain_ContractPattern=1 then 'Single' when BrokerageMain_ContractPattern=2 then 'Sharewise' when BrokerageMain_ContractPattern=3 then 'Orderwise' when BrokerageMain_ContractPattern=4 then 'Order+Sharewise' when BrokerageMain_ContractPattern=5 then 'None' else 'None' end as BrokerageMain_ContractPattern, BrokerageMain_MinDelPerShare, BrokerageMain_MaxDelPerShare, BrokerageMain_MinSqrPerShare, BrokerageMain_MaxSqrPerShare, BrokerageMain_MinDailyBrkg, BrokerageMain_MaxDailyBrkg,  BrokerageMain_MinBrkgPerOrder,BrokerageMain_MaxBrkgPerOrder,CONVERT(VARCHAR(11), BrokerageMain_FromDate , 106) as BrokerageMain_FromDate,BrokerageMain_Type   ", "BrokerageMain_CustomerID='" + dtCHGRUP.Rows[0]["ChargeGroup_Code"].ToString().Trim() + "' and  BrokerageMain_FromDate <= '" + dtCH.Rows[0]["ChargeGroupMembers_FromDate"].ToString().Trim() + "' and (BrokerageMain_UntilDate is  null or BrokerageMain_UntilDate >= '" + dtCH.Rows[0]["ChargeGroupMembers_FromDate"].ToString().Trim() + "') and BrokerageMain_SegmentID='" + seg[s].ToString().Trim() + "'");
                                //  dispTbl = dispTbl + "&nbsp;&nbsp;&nbsp; Type: <b>Scheme [Group Name:" + dtCHGRUP.Rows[0]["ChargeGroup_Name"].ToString() + "[" + dtCHGRUP.Rows[0]["ChargeGroup_Code"].ToString() + "]]</td></tr>";
                                if (dtBRMain.Rows.Count > 0)
                                {
                                    dtCharge = oDBEngine.GetDataTable("Config_ChargeSetup", "case when  ChargeSetup_ChargeType='CL' then 'Clearing Charges' when  ChargeSetup_ChargeType='SD' then 'Stamp Duty' when  ChargeSetup_ChargeType='DM' then 'Demat Charges' when  ChargeSetup_ChargeType='SF' then 'SEBI Fee' when  ChargeSetup_ChargeType='SX' then 'STT' when  ChargeSetup_ChargeType='ST' then 'Service Tax' when  ChargeSetup_ChargeType='TX' then 'Transaction Charge' end as ChargeSetup_ChargeType , case when ChargeSetup_ChargeBasis=1 then 'Inclusive' when ChargeSetup_ChargeBasis=2 then 'Exclusive' when ChargeSetup_ChargeBasis=3 then 'Not Applicable' end  as ChargeSetup_ChargeBasis  , (Select (isnull(ChargeGroup_Name,''))+'[' + ChargeGroup_Code+']'as d from Master_ChargeGroup WHERE  ChargeGroup_Type='2' and ChargeGroup_Code=ChargeSetup_ChargeGroup) as  ChargeSetup_ChargeGroup ", " ChargeSetup_MainID='" + dtBRMain.Rows[0]["BrokerageMain_ID"].ToString().Trim() + "' ");
                                }

                            }
                            //else
                            //{
                            //    dispTbl = dispTbl + "</td></tr>";
                            //}
                        }
                        //else
                        //{
                        //    dispTbl = dispTbl + "</td></tr>";

                        //}
                        if (dtBRMain.Rows.Count > 0)
                        {
                            dtBRDetail = oDBEngine.GetDataTable("Config_BrokerageDetail as cbd ,Config_BrokerageMain as cbm ", "cbd.BrokerageDetail_ProductID,case when cbd.BrokerageDetail_MktSegment=1 then 'All' when  cbd.BrokerageDetail_MktSegment=2 then 'Rolling' when  cbd.BrokerageDetail_MktSegment=3 then 'T2T' when  cbd.BrokerageDetail_MktSegment=4 then 'Physical'  when  cbd.BrokerageDetail_MktSegment=5 then 'Institutional' when cbd.BrokerageDetail_MktSegment=6 then 'Auction' else ' ' end as BrokerageDetail_MktSegment,cbd.BrokerageDetail_SlabCode,cbd.BrokerageDetail_ID,cast(cbd.BrokerageDetail_FlatRate  as decimal(18,2))as BrokerageDetail_FlatRate,cbd.BrokerageDetail_Rate,cast(cbd.BrokerageDetail_MinAmount  as decimal(18,2)) as BrokerageDetail_MinAmount,case when cbd.BrokerageDetail_BrkgFor=1 then 'All'  else (select Products_Name from Master_Products where Products_ID=cbd.BrokerageDetail_ProductID) end as BrokerageDetail_BrkgFor,case when cbd.BrokerageDetail_BrkgType=1 then 'Delivery' when cbd.BrokerageDetail_BrkgType=2 then 'Square-Off' when cbd.BrokerageDetail_BrkgType=3 then 'Exercise' when cbd.BrokerageDetail_BrkgType=4 then 'Assignment' when cbd.BrokerageDetail_BrkgType=6 then 'Delivery CloseValue' else 'Final Settlement' end as BrokerageDetail_BrkgType,case when  cbd.BrokerageDetail_TranType=1 then 'Purchase' when cbd.BrokerageDetail_TranType=2 then 'Sale' when cbd.BrokerageDetail_TranType=3 then 'Both' when cbd.BrokerageDetail_TranType=4 then 'FirstLeg' when cbd.BrokerageDetail_TranType=5 then 'SecondLeg' when cbd.BrokerageDetail_TranType=6 then 'HigherLeg' when cbd.BrokerageDetail_TranType=7  then 'LowerLeg' when cbd.BrokerageDetail_TranType=8 then 'Daily' when cbd.BrokerageDetail_TranType=9 then 'DailySecond' when cbd.BrokerageDetail_TranType=10 then 'Carry' else 'CarrySecond' end as BrokerageDetail_TranType,case when cbd.BrokerageDetail_InstrType=1 then 'All' when cbd.BrokerageDetail_InstrType=2 then 'Equity' when cbd.BrokerageDetail_InstrType=3 then 'Bonds'  when cbd.BrokerageDetail_InstrType=4 then 'Debt' when cbd.BrokerageDetail_InstrType=5 then 'ETFs' when cbd.BrokerageDetail_InstrType=6 then ' Equity Futures' when cbd.BrokerageDetail_InstrType=7 then 'Equity Options'  when cbd.BrokerageDetail_InstrType=8 then 'Index Futures'when cbd.BrokerageDetail_InstrType=9 then 'Index Options'when cbd.BrokerageDetail_InstrType=10 then 'All Futures'when cbd.BrokerageDetail_InstrType=11 then 'All Options' when cbd.BrokerageDetail_InstrType=12 then 'Comm Futures'when cbd.BrokerageDetail_InstrType=13 then 'Comm Options'  when cbd.BrokerageDetail_InstrType=14 then 'All Futures' else 'All Options' end as BrokerageDetail_InstrType ", "cbd.BrokerageDetail_MainID='" + dtBRMain.Rows[0]["BrokerageMain_ID"].ToString() + "' and cbd.BrokerageDetail_MainID=cbm.BrokerageMain_ID");
                        }
                        if (dtBRMain.Rows.Count > 0 && dtBRDetail.Rows.Count > 0 && dtCharge.Rows.Count > 0)
                        {
                            dispTbl = dispTbl + "<tr  style=\"background-color: #99ccff;\"><td  colspan=\"2\" style=\"padding:5px 6px 5px 6px\"><b>Brokerage Setup in " + dtsegName.Rows[0]["SegmentName"].ToString();
                            if (dtCH.Rows[0]["ChargeGroupMembers_GroupType"].ToString().Trim() == "1")
                            {
                                dispTbl = dispTbl + "&nbsp;&nbsp;&nbsp; Type: <b>Specific </td></tr>";
                            }
                            else if (dtCH.Rows[0]["ChargeGroupMembers_GroupType"].ToString().Trim() == "2")
                            {

                                dispTbl = dispTbl + "&nbsp;&nbsp;&nbsp; Type: <b>Scheme [Group Name:" + dtCHGRUP.Rows[0]["ChargeGroup_Name"].ToString() + "[" + dtCHGRUP.Rows[0]["ChargeGroup_Code"].ToString() + "]]</td></tr>";
                            }

                        }

                        if (dtBRMain.Rows.Count > 0)
                        {

                            dispTbl = dispTbl + "<tr style=\"background-color: #ffffff;\" ><td align=\"left\"  valign=\"top\"  style=\"padding:5px 6px 5px 6px\">";

                            dispTbl = dispTbl + "<table  cellpadding=\"1\" cellspacing=\"1\"  style=\"background-color: #ffffff; font-family: verdana; font-size: 12px;color:White;\"><tr style=\"background-color: #ffffff;\">";
                            dispTbl = dispTbl + "<td valign=\"top\">";

                            dispTbl = dispTbl + "<table  cellpadding=\"1\" cellspacing=\"1\"  style=\"background-color: #99ccff; font-family: verdana; font-size: 12px;color:White;\">";
                            dispTbl = dispTbl + "<tr ><td align=\"left\"  valign=\"top\" colspan=\"8\"><b>Brokerage[General] &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<b>Company:" + dtBRMain.Rows[0]["BrokerageMain_CompanyID"].ToString().Trim() + "&nbsp;&nbsp;&nbsp;&nbsp;<b>Date:" + dtBRMain.Rows[0]["BrokerageMain_FromDate"].ToString().Trim() + "</td></tr>";
                            dispTbl = dispTbl + "<tr  style=\"background-color: #ffffff;\"><td align=\"left\"  valign=\"top\">Brkg Decimals:</td><td align=\"left\"  valign=\"top\">" + dtBRMain.Rows[0]["BrokerageMain_BrkgDecimals"].ToString().Trim() + "</td><td align=\"left\"  valign=\"top\">Brkg.Rd-Off:</td><td align=\"left\"  valign=\"top\">" + dtBRMain.Rows[0]["BrokerageMain_BrkgRoundPattern"].ToString().Trim() + "</td><td align=\"left\"  valign=\"top\">Min Daily Brkg:</td><td align=\"left\"  valign=\"top\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dtBRMain.Rows[0]["BrokerageMain_MinDailyBrkg"].ToString().Trim())) + "</td><td align=\"left\"  valign=\"top\">Min Sqr Brkg/Shr:</td><td align=\"left\"  valign=\"top\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dtBRMain.Rows[0]["BrokerageMain_MinSqrPerShare"].ToString().Trim())) + "</td></tr>";
                            dispTbl = dispTbl + "<tr  style=\"background-color: #ffffff;\"><td align=\"left\"  valign=\"top\">Net Amt Decimal:</td><td align=\"left\"  valign=\"top\">" + dtBRMain.Rows[0]["BrokerageMain_NetDecimals"].ToString().Trim() + "</td><td align=\"left\"  valign=\"top\">Mkt. Rd-Off:</td><td align=\"left\"  valign=\"top\">" + dtBRMain.Rows[0]["BrokerageMain_MktRoundPattern"].ToString().Trim() + "</td><td align=\"left\"  valign=\"top\">Max Daily Brkg:</td><td align=\"left\"  valign=\"top\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dtBRMain.Rows[0]["BrokerageMain_MaxDailyBrkg"].ToString().Trim())) + "</td><td align=\"left\"  valign=\"top\">Max Sqr Brkg/Shr:</td><td align=\"left\"  valign=\"top\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dtBRMain.Rows[0]["BrokerageMain_MaxSqrPerShare"].ToString().Trim())) + "</td></tr>";
                            dispTbl = dispTbl + "<tr  style=\"background-color: #ffffff;\"><td align=\"left\"  valign=\"top\">Mkt Rate Decimal:</td><td align=\"left\"  valign=\"top\">" + dtBRMain.Rows[0]["BrokerageMain_MktDecimals"].ToString().Trim() + "</td><td align=\"left\"  valign=\"top\">Net Rd-Off:</td><td align=\"left\"  valign=\"top\">" + dtBRMain.Rows[0]["BrokerageMain_NetRoundPattern"].ToString().Trim() + "</td><td align=\"left\"  valign=\"top\">Min Del Brkg/Shr:</td><td align=\"left\"  valign=\"top\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dtBRMain.Rows[0]["BrokerageMain_MinDelPerShare"].ToString().Trim())) + "</td><td align=\"left\"  valign=\"top\">Min Brkg/Order:</td><td align=\"left\"  valign=\"top\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dtBRMain.Rows[0]["BrokerageMain_MinBrkgPerOrder"].ToString().Trim())) + "</td></tr>";
                            dispTbl = dispTbl + "<tr  style=\"background-color: #ffffff;\"><td align=\"left\"  valign=\"top\">Trd Avg.Type:</td><td align=\"left\"  valign=\"top\">" + dtBRMain.Rows[0]["BrokerageMain_AverageType"].ToString().Trim() + "</td><td align=\"left\"  valign=\"top\">Cont. Pattern:</td><td align=\"left\"  valign=\"top\">" + dtBRMain.Rows[0]["BrokerageMain_ContractPattern"].ToString().Trim() + "</td><td align=\"left\"  valign=\"top\">Max Del Brkg/Shr:</td><td align=\"left\"  valign=\"top\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dtBRMain.Rows[0]["BrokerageMain_MaxDelPerShare"].ToString().Trim())) + "</td><td align=\"left\"  valign=\"top\">Max Brkg/Order:</td><td align=\"left\"  valign=\"top\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dtBRMain.Rows[0]["BrokerageMain_MaxBrkgPerOrder"].ToString().Trim())) + "</td></tr>";
                            dispTbl = dispTbl + "</table>";

                            dispTbl = dispTbl + "</td>";
                            dispTbl = dispTbl + "<td style=\"padding:0px 6px 0px 6px\">";
                            if (dtCharge.Rows.Count > 0)
                            {
                                // dispTbl = dispTbl + "<tr style=\"background-color: #ffffff;\"  align=\"left\"><td align=\"left\" colspan=\"2\"  valign=\"top\"  style=\"padding:5px 6px 5px 6px\">";
                                dispTbl = dispTbl + "<table  cellpadding=\"1\" cellspacing=\"1\" style=\"background-color: #99ccff; font-family: verdana; font-size: 12px;color:White;\"><tr align=\"left\" ><td align=\"left\"  valign=\"top\" colspan=\"3\"><b>Charge Setup[Details]</td></tr><tr  style=\"background-color: #ccffff;\"><td>&nbsp;</td><td>Scheme</td><td>Basis</td></tr>";
                                for (int p = 0; p <= dtCharge.Rows.Count - 1; p++)
                                {
                                    dispTbl = dispTbl + "<tr  style=\"background-color: #ffffff;\"><td align=\"left\">" + dtCharge.Rows[p]["ChargeSetup_ChargeType"].ToString() + "</td><td align=\"left\">" + dtCharge.Rows[p]["ChargeSetup_ChargeGroup"].ToString() + "</td><td align=\"left\">" + dtCharge.Rows[p]["ChargeSetup_ChargeBasis"].ToString() + "</td></tr>";
                                }
                                dispTbl = dispTbl + "</table>";
                                // dispTbl = dispTbl + "</td></tr>";

                            }

                            dispTbl = dispTbl + "</td>";
                            dispTbl = dispTbl + "</table>";



                            dispTbl = dispTbl + "</td></tr>";
                        }
                        if (dtBRDetail.Rows.Count > 0)
                        {
                            dispTbl = dispTbl + "<tr style=\"background-color: #ffffff;\"  align=\"left\"><td align=\"left\" colspan=\"2\"  valign=\"top\"  style=\"padding:5px 6px 5px 6px\">";
                            dispTbl = dispTbl + "<table  cellpadding=\"1\" cellspacing=\"1\" style=\"background-color: #99ccff; font-family: verdana; font-size: 12px;color:White;\"><tr align=\"left\" ><td align=\"left\"  valign=\"top\" colspan=\"9\"><b>Brokerage Setup[Details]:</td></tr><tr  style=\"background-color: #ccffff;\"><td>Market Segment</td><td>Brokerage Type</td><td>Brokerage For</td><td>Transaction Type</td><td>Instrument Type</td><td>Flat Amount</td><td>Rate</td><td>Min. Amount</td><td>Brokerage Slab</td></tr>";
                            for (int n = 0; n <= dtBRDetail.Rows.Count - 1; n++)
                            {
                                dispTbl = dispTbl + "<tr  style=\"background-color: #ffffff;\"><td align=\"left\">" + dtBRDetail.Rows[n]["BrokerageDetail_MktSegment"].ToString() + "</td><td align=\"left\">" + dtBRDetail.Rows[n]["BrokerageDetail_BrkgType"].ToString() + "</td><td align=\"left\">" + dtBRDetail.Rows[n]["BrokerageDetail_BrkgFor"].ToString() + "</td><td align=\"left\">" + dtBRDetail.Rows[n]["BrokerageDetail_TranType"].ToString() + "</td><td align=\"left\">" + dtBRDetail.Rows[n]["BrokerageDetail_InstrType"].ToString() + "</td><td align=\"left\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dtBRDetail.Rows[n]["BrokerageDetail_FlatRate"].ToString())) + "</td><td align=\"left\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dtBRDetail.Rows[n]["BrokerageDetail_Rate"].ToString())) + "</td><td align=\"left\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dtBRDetail.Rows[n]["BrokerageDetail_MinAmount"].ToString())) + "</td><td align=\"left\">" + dtBRDetail.Rows[n]["BrokerageDetail_SlabCode"].ToString() + "</td></tr>";
                            }
                            dispTbl = dispTbl + "</table>";
                            dispTbl = dispTbl + "</td></tr>";

                        }

                        if (dtCPD.Rows.Count > 0)
                        {
                            dispTbl = dispTbl + "<tr style=\"background-color: #ffffff;\"  align=\"left\"><td align=\"left\" colspan=\"2\"  valign=\"top\"  style=\"padding:5px 6px 5px 6px\">";
                            dispTbl = dispTbl + "<table  cellpadding=\"1\" cellspacing=\"1\" style=\"background-color: #99ccff; font-family: verdana; font-size: 12px;color:White;\"><tr align=\"left\" ><td align=\"left\"  valign=\"top\" colspan=\"9\"><b>Contact Person Details:</td></tr><tr  style=\"background-color: #ccffff;\"><td>Name</td><td>Relationship</td><td>Designation</td><td>Phone</td><td>Email</td><td>Status</td><td>PAN Number</td><td>Din</td></tr>";

                            for (int rows = 0; rows < dtCPD.Rows.Count; rows++)
                            {
                                dispTbl = dispTbl + "<tr  style=\"background-color: #ffffff;\"><td align=\"left\">" + Convert.ToString(dtCPD.Rows[rows]["name"]) + "</td>" +
                                    "<td align=\"left\">" + Convert.ToString(dtCPD.Rows[rows]["cp_relationShip"]) + "</td>" +
                                    "<td align=\"left\">" + Convert.ToString(dtCPD.Rows[rows]["cp_designation"]) + "</td>" +
                                    "<td align=\"left\">" + Convert.ToString(dtCPD.Rows[rows]["Phone"]) + "</td>" +
                                    "<td align=\"left\">" + Convert.ToString(dtCPD.Rows[rows]["email"]) + "</td>" +
                                    "<td align=\"left\">" + Convert.ToString(dtCPD.Rows[rows]["cp_status"]) + "</td>" +
                                    "<td align=\"left\">" + Convert.ToString(dtCPD.Rows[rows]["cp_Pan"]) + "</td>" +
                                    "<td align=\"left\">" + Convert.ToString(dtCPD.Rows[rows]["cp_Din"]) + "</td></tr>";

                            }
                            dispTbl = dispTbl + "</table>";
                            dispTbl = dispTbl + "</td></tr>";

                        }




                        dtCH.Clear();
                        dtCharge.Clear();
                        dtBRMain.Clear();
                        dtBRDetail.Clear();
                        dtCHGRUP.Clear();
                        dtCPD.Clear();


                    }


                }




                //-----------Brokerage  --------

                dispTbl = dispTbl + "</table>";
                display.InnerHtml = dispTbl.ToString();
                Session["MainDataTable"] = DtMain;
                HdPageNo.Value = PageNo.ToString();
                if (DtMain.Rows.Count > 1)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Display2", "Display();", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Display12", "height();", true);

                }
                if (DtMain.Rows.Count == 1)
                {
                    // trButton.Visible = false;
                }
                else
                {

                }


            }
        }
        protected void btnFirst_Click(object sender, ImageClickEventArgs e)
        {
            btnFirst.Visible = true;
            btnLast.Visible = true;
            btnPrevious.Visible = false;
            btnNext.Visible = true;
            DtMain = (DataTable)Session["MainDataTable"];
            PageNo = 0;
            fillHTML();
            // Page.ClientScript.RegisterStartupScript(GetType(), "test7", "<script language='javascript'>iframesource('" + DtMain.Rows[PageNo]["cnt_internalid"].ToString().Trim() + "');height();</script>");


        }
        protected void btnPrevious_Click1(object sender, ImageClickEventArgs e)
        {
            PageNo = int.Parse(HdPageNo.Value.ToString()) - 1;
            DtMain = (DataTable)Session["MainDataTable"];
            fillHTML();
            // Page.ClientScript.RegisterStartupScript(GetType(), "test6", "<script language='javascript'>iframesource('" + DtMain.Rows[PageNo]["cnt_internalid"].ToString().Trim() + "');height();</script>");
            if (PageNo == 0)
            {
                btnPrevious.Visible = false;
            }
            else
            {
                btnPrevious.Visible = true;
            }
            btnFirst.Visible = true;
            btnLast.Visible = true;
            btnNext.Visible = true;


        }
        protected void btnNext_Click1(object sender, ImageClickEventArgs e)
        {
            btnFirst.Visible = true;
            //btnPrevious.Visible = false;
            btnLast.Visible = true;
            btnPrevious.Visible = true;
            PageNo = int.Parse(HdPageNo.Value.ToString()) + 1;
            DtMain = (DataTable)Session["MainDataTable"];
            if (PageNo == (int.Parse(DtMain.Rows.Count.ToString()) - 1))
            {
                btnNext.Visible = false;
            }
            else
            {
                btnNext.Visible = true;
            }
            fillHTML();
            //Page.ClientScript.RegisterStartupScript(GetType(), "test5", "<script language='javascript'>iframesource('" + DtMain.Rows[PageNo]["cnt_internalid"].ToString().Trim() + "');height();</script>");

        }
        protected void btnLast_Click1(object sender, ImageClickEventArgs e)
        {
            btnNext.Visible = false;
            btnFirst.Visible = true;
            btnPrevious.Visible = true;
            DtMain = (DataTable)Session["MainDataTable"];
            PageNo = int.Parse(DtMain.Rows.Count.ToString()) - 1;
            fillHTML();
            // Page.ClientScript.RegisterStartupScript(GetType(), "test4", "<script language='javascript'>iframesource('" + DtMain.Rows[PageNo]["cnt_internalid"].ToString().Trim() + "');height();</script>");


        }
        protected void btnPrint_Click(object sender, EventArgs e)
        {
            PageNo = int.Parse(HdPageNo.Value.ToString());
            DtMain = (DataTable)Session["MainDataTable"];
            if (DtMain.Rows.Count > 0)
            {
                if (DtMain.Rows[PageNo]["cnt_internalid"].ToString().Trim().Length > 5)
                {
                    string SegVal = string.Empty;
                    if (rdbSegAll.Checked)
                    {
                        SegVal = "";
                    }
                    else
                    {
                        SegVal = HiddenField_Segment.Value.ToString();
                    }
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "JSct9", "OpenWindow('" + DtMain.Rows[PageNo]["cnt_internalid"].ToString().Trim() + "','" + SegVal + "')", true);
                    // ScriptManager.RegisterStartupScript(this, this.GetType(), "JSct9", "openPopup()", true);
                }
                else
                {

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "JSct50", "alert('Client Not Found!.')", true);
                    // ScriptManager.RegisterStartupScript(this, this.GetType(), "JSct50", "alert('Client Not Found!.')", true);
                }
                // Page.ClientScript.RegisterStartupScript(GetType(), "test", "<script language='javascript'>OpenWindow('" + DtMain.Rows[PageNo]["cnt_internalid"].ToString().Trim() + "');height();</script>");
            }
            else
            {

                ScriptManager.RegisterStartupScript(this, this.GetType(), "JSct10", "alert('Client Not Found!.')", true);
            }
        }
        public void ShowPageNo()
        {


        }


        public void Procedure()
        {

            string IsBranchGroup = "";
            string BranchGroupValue = "";
            string CLIENTS = "";
            if (ddlGroup.SelectedItem.Value.ToString() == "0")
            {
                IsBranchGroup = "Branch";
                if (rdbranchAll.Checked)
                {
                    //Query1 = "select cnt_internalid from tbl_master_contact  where cnt_internalid like 'CL%'";
                    BranchGroupValue = "BranchALL";
                }
                else
                {
                    BranchGroupValue = HiddenField_Branch.Value.ToString().Trim();
                    //Query1 = "select cnt_internalid from tbl_master_contact  where cnt_internalid like 'CL%' and cnt_branchid in (" + HiddenField_Branch.Value.ToString().Trim() + ")";
                }
            }
            else
            {
                IsBranchGroup = "Group";

                if (rdddlgrouptypeAll.Checked)
                {

                    //  Query1 = "select cnt_internalid from tbl_master_contact  where cnt_internalid like 'CL%'";
                    BranchGroupValue = "GroupAll";
                }
                else
                {

                    // Query1 = "select grp_contactid from tbl_trans_group  where grp_contactid like 'CL%' and grp_groupmaster in (" + HiddenField_Group.Value.ToString().Trim() + ")";
                    BranchGroupValue = HiddenField_Group.Value.ToString().Trim();
                }
            }
            if (radPOAClient.Checked == false)
            {
                if (rdbClientALL.Checked)
                {
                    CLIENTS = "ALL";
                }
                else
                {
                    CLIENTS = HiddenField_Client.Value;
                }
            }
            else
            {
                string str = "";
                DataTable dtpoa = oDBEngine.GetDataTable("Select dpd_cntid from tbl_master_contactDPDetails,tbl_master_contact where dpd_POA='1' and dpd_cntid=cnt_internalid");
                for (int m = 0; m < dtpoa.Rows.Count; m++)
                {
                    if (str == "")
                    {
                        str = "'" + dtpoa.Rows[m]["dpd_cntid"].ToString() + "'";
                    }
                    else
                    {
                        str += ",'" + dtpoa.Rows[m]["dpd_cntid"].ToString() + "'";
                    }
                }
                CLIENTS = str;
            }
            ds = mr.Fetch_ClientMaster(Session["LastCompany"].ToString(), rdbSegAll.Checked ? Session["usersegid"].ToString().Trim() : HiddenField_Segment.Value.ToString(),
                IsBranchGroup, BranchGroupValue, dtFrom.Value.ToString(), dtTo.Value.ToString(), ddllist.SelectedItem.Value.ToString() == "0" ? "SHOW" : "PRINT",
                CLIENTS);

            ViewState["dataset"] = ds;
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ddllist.SelectedItem.Value.ToString() == "0")
                {
                    CreateHTML();
                }

                //if (ddllist.SelectedItem.Value.ToString() == "0")
                //{
                //    fillHTML();
                //}
                //else
                //{
                //    print();
                //}

            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "NORECORD", "NORECORD();", true);
            }

            //  }


        }


        protected void btnshow_Click(object sender, EventArgs e)
        {
            if (ddllistType.SelectedItem.Value == "0")
            {
                HdPageNo.Value = "0";
                CreateDataTable();
                if (DtMain.Rows.Count != 0)
                {
                    fillHTML();

                }

                if (DtMain.Rows.Count > 1)
                {
                    // Page.ClientScript.RegisterStartupScript(GetType(), "Message1", "<script language='JavaScript'>Show('trButton');</script>");
                    // trButton.Visible = true;
                }
                if (DtMain.Rows.Count == 0)
                {
                    Page.ClientScript.RegisterStartupScript(GetType(), "Message", "<script language='JavaScript'>alert('No Record To Display');</script>");
                }
                this.Page.ClientScript.RegisterStartupScript(GetType(), "height7", "<script>height();</script>");




            }
            else if (ddllistType.SelectedItem.Value == "1")
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "Message1", "<script language='JavaScript'>Hide('trButton');</script>");
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "Display8", "<script language='JavaScript'>alert('123');</script>", true);
                // Page.ClientScript.RegisterStartupScript(GetType(), "Message4", "<script language='JavaScript'>Hide('trButton');</script>");
                Procedure();
                this.Page.ClientScript.RegisterStartupScript(GetType(), "height124", "<script>height();</script>");
            }


        }



        public void CreateHTML()
        {
            ds = (DataSet)ViewState["dataset"];
            String strHtml = String.Empty;

            strHtml = "<table width=\"2000px\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";
            strHtml += "<tr style=\"background-color: #DBEEF3;\">";
            strHtml += "<td align=\"center\" ><b>Name</b></td>";
            strHtml += "<td align=\"center\" ><b>Branch</b></td>";
            strHtml += "<td align=\"center\" ><b>Category</b></td>";
            strHtml += "<td align=\"center\" ><b>Email</b></td>";
            strHtml += "<td align=\"center\" ><b>Telephone</b></td>";
            strHtml += "<td align=\"center\"  ><b>Mobile</b></td>";
            strHtml += "<td align=\"center\" ><b>Address</b></td>";
            strHtml += "<td align=\"center\"><b>PAN</b></td>";
            strHtml += "<td align=\"center\" ><b>VoterID</b></td>";
            strHtml += "<td align=\"center\" ><b>Ration</b></td>";
            strHtml += "<td align=\"center\" ><b>Passport</b></td>";
            strHtml += "<td align=\"center\" width=\"20%\"><b>Bank Details</b></td>";
            strHtml += "<td align=\"center\" width=\"20%\"><b>DP Details</b></td>";
            strHtml += "<td align=\"center\" ><b>Group</b></td>";
            strHtml += "<td align=\"center\" ><b>Subbroker</b></td>";
            strHtml += "<td align=\"center\"><b>Relationship Partner</b></td>";
            strHtml += "</tr>";



            int flag = 0;

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                flag = flag + 1;
                strHtml += "<tr valign=top id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
                //strHtml += "<td align=\"left\" >&nbsp;" + ds.Tables[0].Rows[i][].ToString() + "</td>";
                //  strHtml += "<td align=\"left\" style=\"font-size:xx-small;\" >&nbsp;" + ds.Tables[0].Rows[i][1].ToString() + "</td>";
                strHtml += "<td align=\"left\" style=\"font-size:xx-small;\" >&nbsp;" + ds.Tables[0].Rows[i][2].ToString() + "</td>";
                strHtml += "<td align=\"left\" style=\"font-size:xx-small;\" >&nbsp;" + ds.Tables[0].Rows[i][3].ToString() + "</td>";
                strHtml += "<td align=\"left\" style=\"font-size:xx-small;\" >&nbsp;" + ds.Tables[0].Rows[i][4].ToString() + "</td>";
                strHtml += "<td align=\"left\" style=\"font-size:xx-small;\" >&nbsp;" + ds.Tables[0].Rows[i][5].ToString() + "</td>";
                strHtml += "<td align=\"left\" style=\"font-size:xx-small;\" >&nbsp;" + ds.Tables[0].Rows[i][6].ToString() + "</td>";
                strHtml += "<td align=\"left\" style=\"font-size:xx-small;\" >&nbsp;" + ds.Tables[0].Rows[i][7].ToString() + "</td>";
                strHtml += "<td align=\"left\" style=\"font-size:xx-small;\" >&nbsp;" + ds.Tables[0].Rows[i][8].ToString() + "</td>";
                strHtml += "<td align=\"left\" style=\"font-size:xx-small;\" >&nbsp;" + ds.Tables[0].Rows[i][9].ToString() + "</td>";
                strHtml += "<td align=\"left\" style=\"font-size:xx-small;\" >&nbsp;" + ds.Tables[0].Rows[i][10].ToString() + "</td>";
                strHtml += "<td align=\"left\" style=\"font-size:xx-small;\" >&nbsp;" + ds.Tables[0].Rows[i][11].ToString() + "</td>";
                strHtml += "<td align=\"left\" style=\"font-size:xx-small;\" >&nbsp;" + ds.Tables[0].Rows[i][12].ToString() + "</td>";
                strHtml += "<td align=\"left\" style=\"font-size:xx-small;\" >&nbsp;" + ds.Tables[0].Rows[i][13].ToString() + "</td>";
                strHtml += "<td align=\"left\" style=\"font-size:xx-small;\" >&nbsp;" + ds.Tables[0].Rows[i][14].ToString() + "</td>";
                strHtml += "<td align=\"left\" style=\"font-size:xx-small;\" >&nbsp;" + ds.Tables[0].Rows[i][15].ToString() + "</td>";
                strHtml += "<td align=\"left\" style=\"font-size:xx-small;\" >&nbsp;" + ds.Tables[0].Rows[i][16].ToString() + "</td>";
                strHtml += "<td align=\"left\" style=\"font-size:xx-small;\" >&nbsp;" + ds.Tables[0].Rows[i][17].ToString() + "</td>";
                strHtml += "</tr>";

                /////////BANK DETAILS
                DataView view1 = new DataView();
                view1 = ds.Tables[1].DefaultView;
                view1.RowFilter = "ChargeGroupMembers_CustomerID='" + ds.Tables[0].Rows[i]["ClientID"].ToString().Trim() + "'";

                DataTable dt2 = new DataTable();
                dt2 = view1.ToTable();
                strHtml += "<tr>  <td colspan=\"16\" align=\"left\">";
                strHtml += "<table width=\"600px\" border=" + Convert.ToChar(34) + 1 + Convert.ToChar(34) + " cellpadding=" + 0 + " cellspacing=" + 0 + " class=" + Convert.ToChar(34) + "gridcellleft" + Convert.ToChar(34) + ">";
                strHtml += "<tr valign=top id=\"tr_id" + flag + "&" + GetRowColor(flag) + "\" onclick=heightlight(this.id) style=\"background-color: " + GetRowColor(flag) + " ;text-align:center\">";
                strHtml += "<td align=\"center\" ><b>Segment</b></td>";
                strHtml += "<td align=\"center\" ><b>TCODE</b></td>";
                strHtml += "<td align=\"center\" ><b>Registration Date</b></td>";
                strHtml += "<td align=\"center\" ><b>Brokerage</b></td>";
                strHtml += "<td align=\"center\"  ><b>Stump Duty</b></td>";
                strHtml += "<td align=\"center\" ><b>Demat Charges</b></td>";
                strHtml += "<td align=\"center\"><b>SEBI Fee</b></td>";
                strHtml += "<td align=\"center\" ><b>STT</b></td>";
                strHtml += "<td align=\"center\" ><b>Transaction Charges</b></td>";
                strHtml += "<td align=\"center\" ><b>Service Tax</b></td>";
                strHtml += "</tr>";

                for (int k = 0; k < dt2.Rows.Count; k++)
                {
                    strHtml += "<tr>";

                    strHtml += "<td align=\"left\" style=\"font-size:xx-small;\" >&nbsp;" + dt2.Rows[k]["crg_exchange"].ToString() + "</td>";
                    strHtml += "<td align=\"left\" style=\"font-size:xx-small;\" >&nbsp;" + dt2.Rows[k]["crg_tcode"].ToString() + "</td>";

                    strHtml += "<td align=\"left\" style=\"font-size:xx-small;\" >&nbsp;" + dt2.Rows[k]["crg_regisDate1"].ToString() + "</td>";
                    strHtml += "<td align=\"left\" style=\"font-size:xx-small;\" >&nbsp;" + dt2.Rows[k]["Brok"].ToString() + "</td>";
                    strHtml += "<td align=\"left\" style=\"font-size:xx-small;\" >&nbsp;" + dt2.Rows[k]["StampDuty"].ToString() + "</td>";
                    strHtml += "<td align=\"left\" style=\"font-size:xx-small;\" >&nbsp;" + dt2.Rows[k]["DematCharges"].ToString() + "</td>";
                    strHtml += "<td align=\"left\" style=\"font-size:xx-small;\" >&nbsp;" + dt2.Rows[k]["SEBIFee"].ToString() + "</td>";
                    strHtml += "<td align=\"left\" style=\"font-size:xx-small;\" >&nbsp;" + dt2.Rows[k]["STT"].ToString() + "</td>";
                    strHtml += "<td align=\"left\" style=\"font-size:xx-small;\" >&nbsp;" + dt2.Rows[k]["TransactionCharge"].ToString() + "</td>";
                    strHtml += "<td align=\"left\" style=\"font-size:xx-small;\" >&nbsp;" + dt2.Rows[k]["ServiceTax"].ToString() + "</td>";


                    strHtml += "</tr>";

                }

                strHtml += "</table></td></tr>";

            }
            strHtml += "</table>";


            display.InnerHtml = strHtml;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Display", "DisplayCon();", true);

        }

        protected string GetRowColor(int i)
        {
            if (i++ % 2 == 0)
                return "#fff0f5";
            else
                return "White";
        }

        void print()
        {
            ds = (DataSet)ViewState["dataset"];
            byte[] logoinByte;
            ds.Tables[0].Columns.Add("Image", System.Type.GetType("System.Byte[]"));
            if (oconverter.getLogoImage(HttpContext.Current.Server.MapPath(@"..\images\logo.bmp"), out logoinByte) != 1)
            {
                ScriptManager.RegisterStartupScript(HttpContext.Current.Handler as Page, HttpContext.Current.GetType(), "logo", "alert('Logo not Found.');", true);
            }
            else
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    ds.Tables[0].Rows[i]["Image"] = logoinByte;

                }
            }
            ReportDocument report = new ReportDocument();
            //ds.Tables[0].WriteXmlSchema("E:\\RPTXSD\\ClientMasterMain.xsd");
            //ds.Tables[1].WriteXmlSchema("E:\\RPTXSD\\ClientMasterMainDetail.xsd");

            report.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Landscape;
            string tmpPdfPath = string.Empty;
            tmpPdfPath = HttpContext.Current.Server.MapPath("..\\Reports\\ClientMasterMain.rpt");
            report.Load(tmpPdfPath);
            report.SetDataSource(ds.Tables[0]);
            report.Subreports["ClientMasterDetails"].SetDataSource(ds.Tables[1]);
            //report.Subreports["dpdetails"].SetDataSource(ds.Tables[2]);
            report.VerifyDatabase();
            report.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.Excel, HttpContext.Current.Response, true, "Client Master Main");

            report.Dispose();
            GC.Collect();
        }

        protected void btnprint_Click(object sender, EventArgs e)
        {

            if (ddllistType.SelectedItem.Value == "0")
            {

                HdPageNo.Value = "0";
                CreateDataTable();

                if (DtMain.Rows.Count != 0)
                {

                    //GeneratePDF();
                    GeneratePDFNew();

                }

                if (DtMain.Rows.Count == 0)
                {
                    Page.ClientScript.RegisterStartupScript(GetType(), "Message", "<script language='JavaScript'>alert('No Record To Display');</script>");
                }
                this.Page.ClientScript.RegisterStartupScript(GetType(), "height123", "<script>height();</script>");

            }
            else if (ddllistType.SelectedItem.Value == "1")
            {

                Procedure();
                print();
                this.Page.ClientScript.RegisterStartupScript(GetType(), "height123", "<script>height();</script>");
            }
        }

        public void GeneratePDFNew()
        {

            Winnovative.PdfCreator.LicensingManager.LicenseKey = "jKe9rL20rLq1u6y/orysv72ivb6itbW1tQ==";
            Winnovative.PdfCreator.Document doc = null;
            if (ddllist.SelectedValue == "1")
            {
                doc = new Winnovative.PdfCreator.Document();
                doc.CompressionLevel = Winnovative.PdfCreator.CompressionLevel.NormalCompression;
            }
            // Document doc = new Document(iTextSharp.text.PageSize.LETTER, 10, 10, 42, 35);
            Winnovative.PdfCreator.PdfPage page = null;
            Winnovative.PdfCreator.AddElementResult addResult;
            Winnovative.PdfCreator.HtmlToPdfElement htmlToPdfElement;
            Winnovative.PdfCreator.PdfFont font;

            string strCompAddrLogo = "";

            string strCompAddr = "";
            string strLogoPath = "";
            string strCompLogo = "";
            if (chkComplogo.Checked == true)
            {
                string[,] compid = oDBEngine.GetFieldValue("tbl_master_company", "cmp_id", "cmp_internalid='" + Session["LastCompany"].ToString() + "'", 1);
                strLogoPath = Server.MapPath("../images/") + "logo_" + compid[0, 0] + ".BMP";
                strCompLogo = "<img alt='' src='" + strLogoPath + "' style='border-width:0px' />";
            }
            try
            {
                string pdfFilePath = "Test";
                //DataTable dtCompany = oDBEngine.GetDataTable("tbl_master_company,tbl_master_address,tbl_master_country,tbl_master_state,tbl_master_city,tbl_master_phonefax,tbl_master_email", "cmp_Name,add_address1,add_address2,add_address3,add_pin,cou_country,state,city_name,phf_Phonenumber,eml_email,eml_website", "cmp_internalid=add_cntId and add_country=cou_id and add_state=id and add_city=city_id and add_cntId=phf_cntid and add_cntId=eml_cntid  and add_entity='Company' and add_cntId='" + Session["LastCompany"].ToString() + "'");
                DataTable dtCompany = oDBEngine.GetDataTable("tbl_master_company ,tbl_master_address,tbl_master_country,tbl_master_state,tbl_master_city,tbl_master_phonefax,tbl_master_email", "cmp_Name,add_address1,add_address2,add_address3,add_pin,cou_country,state,city_name,phf_Phonenumber,eml_email,eml_website", "cmp_internalid=add_cntId and add_country=cou_id and add_state=id and add_city=city_id and add_cntId=phf_cntid and add_cntId=eml_cntid and add_entity='Company' and eml_entity='Company' and phf_type='Office' and add_cntId='" + Session["LastCompany"].ToString() + "'");
                //strCompAddr = "<table><tr><td></td></tr></table>";
                if (chkCompAddr.Checked == true)
                {
                    strCompAddr = Convert.ToString(dtCompany.Rows[0]["cmp_Name"]) + "</br>" + Convert.ToString(dtCompany.Rows[0]["add_address1"]);
                    if (Convert.ToString(dtCompany.Rows[0]["add_address2"]) != "")
                        strCompAddr = strCompAddr + " " + Convert.ToString(dtCompany.Rows[0]["add_address2"]);
                    if (Convert.ToString(dtCompany.Rows[0]["add_address3"]) != "")
                        strCompAddr = strCompAddr + "</br>" + Convert.ToString(dtCompany.Rows[0]["add_address3"]);
                    strCompAddr = strCompAddr + "</br>" + Convert.ToString(dtCompany.Rows[0]["city_name"]) + "-" + Convert.ToString(dtCompany.Rows[0]["add_pin"]);

                    if (Convert.ToString(dtCompany.Rows[0]["phf_Phonenumber"]) != "")
                        strCompAddr = strCompAddr + "</br>" + "Phones: " + Convert.ToString(dtCompany.Rows[0]["phf_Phonenumber"]);

                    if (Convert.ToString(dtCompany.Rows[0]["eml_email"]) != "")
                        strCompAddr = strCompAddr + "</br>" + "Email: " + Convert.ToString(dtCompany.Rows[0]["eml_email"]);

                    if (Convert.ToString(dtCompany.Rows[0]["eml_website"]) != "")
                        strCompAddr = strCompAddr + "</br>" + "Web:" + Convert.ToString(dtCompany.Rows[0]["eml_website"]);
                }
                if (chkComplogo.Checked == true || chkCompAddr.Checked == true)
                    strCompAddrLogo = "<table width='100%'><tr><td  style='text-align: center; width:100%'><table width='815px' border='0' cellpadding='1px' cellspacing='0'><tr><td style='width:400px; vertical-align:top'>" + strCompLogo + "</td><td style='font-family:Times New Roman; font-size:19px'>" + strCompAddr + "</td></tr></table></td></tr></table></br>";

                //Winnovative.PdfCreator.PdfFont pfont8 = doc.Fonts.Add(new System.Drawing.Font(new System.Drawing.FontFamily("ARIAL"), 7, System.Drawing.GraphicsUnit.Point));

                //Winnovative.PdfCreator.PdfFont pfont9 = doc.Fonts.Add(new System.Drawing.Font(new System.Drawing.FontFamily("Times New Roman"), 8, System.Drawing.GraphicsUnit.Point));


                //Font font8 = FontFactory.GetFont("ARIAL", 7);
                //Font font9 = FontFactory.GetFont("TIMES_ROMAN", BaseFont.WINANSI, 8, Font.BOLD, CMYKColor.BLACK);
                for (int b = 0; b < DtMain.Rows.Count; b++)
                {
                    //doc.NewPage();
                    if (ddllist.SelectedValue == "2")
                    {
                        doc = new Winnovative.PdfCreator.Document();
                        doc.CompressionLevel = Winnovative.PdfCreator.CompressionLevel.NormalCompression;
                    }


                    string strBankDetails = "";
                    string strDPDetails = "";
                    string strGroup = "";
                    string strExchange = "";
                    string strStatutory = "";
                    string strHeaderTable = "";
                    string strBrokHead = "";
                    string strBrokertageGen = "";
                    string strChargeSetUpDetails = "";
                    string strBrokerageSetUpDetails = "";
                    string strBrokerageDetailsFull = "";
                    string strAddSignature = "";
                    string strPrintedDateTime = "";
                    string strContactDetails = "";

                    DataTable dt = new DataTable();
                    DataTable dtcomm = new DataTable();
                    DataTable dtcm = new DataTable();
                    DataTable dtCPD = new DataTable();
                    //if ((HttpContext.Current.Session["ExchangeSegmentID"].ToString().Trim() == "1") ||  ||  ||  ||  ||  ||  ||  ||  ||


                    dtCPD = oDBEngine.GetDataTable("select  A.cp_contactId as ContactId, A.cp_name as name,(select phf_phonenumber from tbl_master_phonefax where phf_cntId=A.cp_contactId and phf_type='Office') as Officephone," +
                            "(select phf_phonenumber from tbl_master_phonefax where phf_cntId=A.cp_contactId and phf_type='Residence') as Residencephone," +
                            "(select phf_phonenumber from tbl_master_phonefax where phf_cntId=A.cp_contactId and phf_type='Mobile') as Mobilephone," +
                            "isnull(('(O)'+(select phf_phonenumber from tbl_master_phonefax where phf_cntId=A.cp_contactId and phf_type='Office')),'')+" +
                            "isnull(('(R)'+(select phf_phonenumber from tbl_master_phonefax where phf_cntId=A.cp_contactId and phf_type='Residence')),'')+" +
                            "isnull(('(M)'+(select phf_phonenumber from tbl_master_phonefax where phf_cntId=A.cp_contactId and phf_type='Mobile')),'') as Phone," +
                            "(select eml_email from tbl_master_email where eml_cntId=A.cp_contactId) as email, (case when cp_status = 'Y' then 'Active' " +
                            "when cp_status = 'N' then 'Suspended' else 'N/A'  end) as status, ltrim(rtrim(cp_status)) as cp_status," +
                            "(select deg_designation from tbl_master_designation where deg_id= ltrim(rtrim(  cp_designation))) as  cp_designation," +
                            "(select fam_familyRelationship from tbl_master_familyrelationship where fam_id=ltrim(rtrim( cp_relationShip))) as cp_relationShip," +
                            "cp_Pan,cp_Din from tbl_master_contactperson A  where cp_agentInternalId in('" + DtMain.Rows[b]["cnt_internalid"].ToString().Trim() + "') ORDER BY cp_status desc");



                    dt = oDBEngine.GetDataTable("tbl_master_contactDPDetails", "dpd_accountType as AccountType,case when dpd_POA=1 then 'YES' else 'NO' end as IsPOA, dpd_dpCode as dipositoryID,(select dp_dpName from tbl_master_depositoryParticipants  where left(dp_dpid,8)=left(dpd_dpCode,8)) as DipositoryName ,dpd_ClientId as  BenOwnerAccNo", "dpd_cntID='" + DtMain.Rows[b]["cnt_internalid"].ToString().Trim() + "' and (dpd_accounttype='Default' or dpd_accounttype='Secondary') ");
                    dtcomm = oDBEngine.GetDataTable("tbl_master_contactDPDetails", "dpd_accountType as AccountType,case when dpd_POA=1 then 'YES' else 'NO' end as IsPOA, dpd_dpCode as dipositoryID,(select dp_dpName from tbl_master_depositoryParticipants  where left(dp_dpid,8)=left(dpd_dpCode,8)) as DipositoryName ,dpd_ClientId as  BenOwnerAccNo", "dpd_cntID='" + DtMain.Rows[b]["cnt_internalid"].ToString().Trim() + "' and (dpd_accounttype='CommodityDP' or dpd_accounttype='CommodityDP Sec') ");
                    dtcm = dt = oDBEngine.GetDataTable("tbl_master_contactDPDetails", "dpd_accountType as AccountType,case when dpd_POA=1 then 'YES' else 'NO' end as IsPOA, dpd_dpCode as dipositoryID,(select dp_dpName from tbl_master_depositoryParticipants  where left(dp_dpid,8)=left(dpd_dpCode,8)) as DipositoryName ,dpd_ClientId as  BenOwnerAccNo", "dpd_cntID='" + DtMain.Rows[b]["cnt_internalid"].ToString().Trim() + "'");
                    DataTable dtGeneral = oDBEngine.GetDataTable("tbl_master_contact", "top 1 isnull(ltrim(rtrim(cnt_ucc)),'') as [ClientCode]," +
                       "isnull(ltrim(rtrim(cnt_firstname)),'')+' '+ isnull(ltrim(rtrim(cnt_middlename)),'') +' '+isnull(ltrim(rtrim(cnt_lastname)),'')  as [ClientName] ," +
                       "(select  lgl_legalStatus  from tbl_master_legalstatus where lgl_id=cnt_legalStatus ) as [Category]," +
                       "(select  cntstu_contactstatus  from tbl_master_contactStatus where cntstu_id=cnt_contactStatus ) as [Status]," +
                       "(select top 1 isnull(crg_Number,' ') from tbl_master_contactRegistration where crg_cntID=cnt_internalID and crg_type='Pancard') as [PanNo]," +
                       "(select top 1 eml_email from tbl_master_email where  eml_cntId=cnt_internalID and eml_type='Official') as [ClientEmail]," +
                       "(select top 1  isnull(ltrim(rtrim(phf_countryCode)),'') from tbl_master_phonefax  where phf_cntId=cnt_internalID) as [ISDCode]," +
                       "(select top 1  isnull(ltrim(rtrim(phf_areaCode)),'') from tbl_master_phonefax  where phf_cntId=cnt_internalID) as [STDCode]," +
                       "(select top 1  isnull(ltrim(rtrim(phf_phoneNumber)),'') from tbl_master_phonefax  where phf_cntId=cnt_internalID and phf_type<>'Mobile') as [TelephoneNumber]," +
                       "(select top 1  isnull(ltrim(rtrim(phf_phoneNumber)),'') from tbl_master_phonefax  where phf_cntId=cnt_internalID and phf_type='Mobile') as [MobNumber]," +
                       "REPLACE(CONVERT(VARCHAR(11), cnt_dOB, 106), ' ', '-')  as DateOfBirth," +
                       "(select top 1 REPLACE(CONVERT(VARCHAR(11), crg_regisDate, 106), ' ', '-') from tbl_master_contactExchange  where crg_cntID=cnt_internalID) as ClientAgrementDate," +
                       "(select top 1 isnull(crg_Number,' ') from tbl_master_contactRegistration where crg_cntID=cnt_internalID and crg_type='Other' and crg_registrationAuthority='ROC') as  RegNo," +
                       "(select top 1 isnull(crg_registrationAuthority,' ') from tbl_master_contactRegistration where crg_cntID=cnt_internalID and crg_type='Other' and crg_registrationAuthority='ROC') as  RegAuthority," +
                       "(select top 1 isnull(crg_place,' ') from tbl_master_contactRegistration where crg_cntID=cnt_internalID and crg_type='Other' and crg_registrationAuthority='ROC') as  RegPlace," +
                       "(select CONVERT(VARCHAR(20), GETDATE(), 106)+SUBSTRING(CONVERT(VARCHAR(20), GETDATE(), 100),13,LEN(CONVERT(VARCHAR(20), GETDATE())))) as PrintedDateTime," +
                       "(select top 1 isnull(REPLACE(CONVERT(VARCHAR(11), cnt_dOB, 106), ' ', '-'),' ') from tbl_master_contactRegistration where crg_cntID=cnt_internalID and crg_type='Other' and crg_registrationAuthority='ROC') as RegistrationDate", "cnt_internalID='" + DtMain.Rows[b]["cnt_internalid"].ToString().Trim() + "'");

                    DataTable DtAdd = new DataTable();


                    DtAdd = oDBEngine.GetDataTable("tbl_master_address", " isnull(ltrim(rtrim(add_address1)),'')  as Addres1, isnull(ltrim(rtrim(add_address2)),'')  as Addres2, isnull(ltrim(rtrim(add_address3)),'') as Addres3,(select isnull(ltrim(rtrim(city_name)),'') from tbl_master_City  where city_id =add_City) as City,(select isnull(ltrim(rtrim(state)),'') from tbl_master_state  where id= add_state) as State,(select isnull(ltrim(rtrim(cou_country)),'') from tbl_master_country  where cou_id=add_country ) as Country, isnull(ltrim(rtrim(add_pin)),'')  as Pin", " add_addressType='Registered'  and  add_cntId='" + DtMain.Rows[b]["cnt_internalid"].ToString().Trim() + "'");
                    if (DtAdd.Rows.Count == 0)
                    {
                        DtAdd = oDBEngine.GetDataTable("tbl_master_address", "top 1 isnull(ltrim(rtrim(add_address1)),'')  as Addres1, isnull(ltrim(rtrim(add_address2)),'')  as Addres2, isnull(ltrim(rtrim(add_address3)),'') as Addres3,(select isnull(ltrim(rtrim(city_name)),'') from tbl_master_City  where city_id =add_City) as City,(select isnull(ltrim(rtrim(state)),'') from tbl_master_state  where id= add_state) as State,(select isnull(ltrim(rtrim(cou_country)),'') from tbl_master_country  where cou_id=add_country ) as Country, isnull(ltrim(rtrim(add_pin)),'')  as Pin", " add_cntId='" + DtMain.Rows[b]["cnt_internalid"].ToString().Trim() + "'");

                    }

                    DataTable dp = oDBEngine.GetDataTable("tbl_master_contactDPDetails", "dpd_accountType as AccountType,case when dpd_POA=1 then 'YES' else 'NO' end as IsPOA, dpd_dpCode as dipositoryID,(select dp_dpName from tbl_master_depositoryParticipants  where left(dp_dpid,8)=left(dpd_dpCode,8)) as DipositoryName ,dpd_ClientId as  BenOwnerAccNo", "dpd_cntID='" + DtMain.Rows[b]["cnt_internalid"].ToString().Trim() + "'");
                    DataTable dtBank = oDBEngine.GetDataTable("tbl_trans_contactBankDetails ,tbl_master_Bank ", " cbd_accountCategory as Category, tbl_master_Bank.bnk_bankName AS BankName, tbl_master_Bank.bnk_micrno as BankMICR,tbl_master_Bank.bnk_IFSCCODE as BankIFSC,(tbl_master_Bank.bnk_bankName + ' ' + tbl_master_Bank.bnk_branchName + ' ' + tbl_master_Bank.bnk_micrno ) AS BranchAdress,  tbl_trans_contactBankDetails.cbd_accountType  as AccountType, tbl_trans_contactBankDetails.cbd_accountNumber AS AccountNumber", "(tbl_trans_contactBankDetails.cbd_cntId ='" + DtMain.Rows[b]["cnt_internalid"].ToString().Trim() + "') and  tbl_master_Bank.bnk_id=tbl_trans_contactBankDetails.cbd_bankCode");
                    DataTable dtProof = oDBEngine.GetDataTable("tbl_master_contactRegistration", " crg_type, crg_Number", "crg_cntID='" + DtMain.Rows[b]["cnt_internalid"].ToString().Trim() + "'");
                    DataTable dtGroup = oDBEngine.GetDataTable(" tbl_trans_group INNER JOIN tbl_master_groupMaster ON tbl_trans_group.grp_groupMaster = tbl_master_groupMaster.gpm_id ", "tbl_master_groupMaster.gpm_Type as GroupType,ltrim(rtrim(tbl_master_groupMaster.gpm_Description)) +'['+rtrim(ltrim(tbl_master_groupMaster.gpm_code))+']' as GroupName  ", "tbl_trans_group.grp_contactId='" + DtMain.Rows[b]["cnt_internalid"].ToString().Trim() + "'");

                    DataTable dtIntd = oDBEngine.GetDataTable("tbl_master_contact", "TOP 1 ltrim(rtrim(isnull(cnt_firstname,'')))+' '+ltrim(rtrim(isnull(cnt_Middlename,'')))+' '+ltrim(rtrim(isnull(cnt_LastName,' ')))  as IntdName,CASE WHEN   (cnt_ucc=NULL OR cnt_ucc='') THEN  cnt_shortName ELSE cnt_ucc END AS  IntdUCC,(select top 1 isnull(crg_number,' ') from tbl_master_contactregistration  where crg_cntid=cnt_INTERNALID and crg_type='Pancard') as IntdPAN ", "CNT_INTERNALID IN(SELECT CNT_REFEREDBY FROM TBL_MASTER_CONTACT WHERE CNT_INTERNALID='" + DtMain.Rows[b]["cnt_internalid"].ToString().Trim() + "') ");


                    DataTable dtSeg = new DataTable();
                    if (rdbSegAll.Checked == true)
                    {
                        dtSeg = oDBEngine.GetDataTable("tbl_master_contactExchange ce ", "ce.crg_exchange as crg_exchange,ltrim(rtrim(ce.crg_tcode)) as crg_tcode,case when ce.crg_regisDate='1/1/1900 12:00:00 AM' then null else convert(varchar(11),ce.crg_regisDate,106) end as crg_regisDate,case when ce.crg_businessCmmDate='1/1/1900 12:00:00 AM' then null else convert(varchar(11),ce.crg_businessCmmDate,106) end as crg_businessCmmDate,case when ce.crg_suspensionDate='1/1/1900 12:00:00 AM' then null else convert(varchar(11),ce.crg_suspensionDate,106) end as crg_suspensionDate ", "crg_cntID='" + DtMain.Rows[b]["cnt_internalid"].ToString().Trim() + "' and crg_company='" + Session["LastCompany"].ToString() + "' ");
                    }
                    else
                    {
                        string Seg = "'N'";
                        string[] SegmentName = HiddenField_SegmentName.Value.ToString().Split(',');
                        for (int n = 0; n < SegmentName.Length; n++)
                        {
                            Seg = Seg + ",'" + SegmentName[n] + "'";

                        }
                        dtSeg = oDBEngine.GetDataTable("tbl_master_contactExchange ce ", "ce.crg_exchange as crg_exchange,ltrim(rtrim(ce.crg_tcode)) as crg_tcode,case when ce.crg_regisDate='1/1/1900 12:00:00 AM' then null else convert(varchar(11),ce.crg_regisDate,106) end as crg_regisDate,case when ce.crg_businessCmmDate='1/1/1900 12:00:00 AM' then null else convert(varchar(11),ce.crg_businessCmmDate,106) end as crg_businessCmmDate,case when ce.crg_suspensionDate='1/1/1900 12:00:00 AM' then null else convert(varchar(11),ce.crg_suspensionDate,106) end as crg_suspensionDate ", "crg_cntID='" + DtMain.Rows[b]["cnt_internalid"].ToString().Trim() + "' and  crg_exchange in (" + Seg + ")  ");
                    }

                    // DataTable dtSeg = oDBEngine.GetDataTable("tbl_master_contactExchange ce ", "ce.crg_exchange as crg_exchange,ltrim(rtrim(ce.crg_tcode)) as crg_tcode,case when ce.crg_regisDate='1/1/1900 12:00:00 AM' then null else convert(varchar(11),ce.crg_regisDate,106) end as crg_regisDate", "crg_cntID='" + DtMain.Rows[b]["cnt_internalid"].ToString().Trim() + "'");
                    DataTable dtHdr = new DataTable();
                    DataTable dtFtr = new DataTable();
                    if (chkHeader.Checked == true)
                    {
                        if (txtHeader_hidden.Value != "")
                        {
                            string Templ = oDBEngine.GenerateGenericTemplate(txtHeader_hidden.Value.ToString(), DtMain.Rows[b]["cnt_internalid"].ToString());
                            ////  Templ = " <p class='MsoNormal' style='MARGIN: 0in 0in 10pt; LINE-HEIGHT: normal; mso-margin-top-alt: auto; mso-margin-bottom-alt: auto'>      <span style='FONT-SIZE: 10pt; FONT-FAMILY: ''Palatino Linotype'',''serif''; mso-fareast-font-family: ''Times New Roman''; mso-bidi-font-family: ''Times New Roman'''>#FirstName# #MiddleName# #LastName# <?xml:namespace prefix = o ns = 'urn:schemas-microsoft-com:office:office' /?><o:p></o:p></span>    </p>    <p class='MsoNormal' style='MARGIN: 0in 0in 10pt; LINE-HEIGHT: normal; mso-margin-top-alt: auto; mso-margin-bottom-alt: auto'>      <span style='FONT-SIZE: 10pt; FONT-FAMILY: ''Palatino Linotype'',''serif''; mso-fareast-font-family: ''Times New Roman''; mso-bidi-font-family: ''Times New Roman'''>#Addres1# <o:p></o:p></span>    </p>    <p class='MsoNormal' style='MARGIN: 0in 0in 10pt; LINE-HEIGHT: normal; mso-margin-top-alt: auto; mso-margin-bottom-alt: auto'>      <span style='FONT-SIZE: 10pt; FONT-FAMILY: ''Palatino Linotype'',''serif''; mso-fareast-font-family: ''Times New Roman''; mso-bidi-font-family: ''Times New Roman'''>#Addres2#<o:p></o:p></span>    </p>    <p class='MsoNormal' style='MARGIN: 0in 0in 10pt; LINE-HEIGHT: normal; mso-margin-top-alt: auto; mso-margin-bottom-alt: auto'>      <span style='FONT-SIZE: 10pt; FONT-FAMILY: ''Palatino Linotype'',''serif''; mso-fareast-font-family: ''Times New Roman''; mso-bidi-font-family: ''Times New Roman'''>#Addres3#  <o:p></o:p></span>    </p>    <p class='MsoNormal' style='MARGIN: 0in 0in 10pt; LINE-HEIGHT: normal; mso-margin-top-alt: auto; mso-margin-bottom-alt: auto'>      <span style='FONT-SIZE: 10pt; FONT-FAMILY: ''Palatino Linotype'',''serif''; mso-fareast-font-family: ''Times New Roman''; mso-bidi-font-family: ''Times New Roman'''>#City#-#Pin# <o:p></o:p></span>    </p>    <p class='MsoNormal' style='MARGIN: 0in 0in 10pt; LINE-HEIGHT: normal; mso-margin-top-alt: auto; mso-margin-bottom-alt: auto'>      <span style='FONT-SIZE: 10pt; FONT-FAMILY: ''Palatino Linotype'',''serif''; mso-fareast-font-family: ''Times New Roman''; mso-bidi-font-family: ''Times New Roman'''>#State# #Country# <o:p></o:p></span>    </p>    <p class='MsoNormal' style='MARGIN: 0in 0in 10pt; LINE-HEIGHT: normal; mso-margin-top-alt: auto; mso-margin-bottom-alt: auto'>      <span style='FONT-SIZE: 10pt; FONT-FAMILY: ''Palatino Linotype'',''serif''; mso-fareast-font-family: ''Times New Roman''; mso-bidi-font-family: ''Times New Roman'''>Dear #FirstName# #MiddleName# #LastName# <o:p></o:p></span>    </p>    <p class='MsoNormal' style='MARGIN: 0in 0in 10pt; LINE-HEIGHT: normal; mso-margin-top-alt: auto; mso-margin-bottom-alt: auto'>      <span style='FONT-SIZE: 12pt; FONT-FAMILY: ''Times New Roman'',''serif''; mso-fareast-font-family: ''Times New Roman'''> <o:p></o:p></span>    </p>    <p class='MsoNormal' style='MARGIN: 0in 0in 10pt; LINE-HEIGHT: normal; mso-margin-top-alt: auto; mso-margin-bottom-alt: auto'>      <span style='FONT-SIZE: 10pt; FONT-FAMILY: ''Palatino Linotype'',''serif''; mso-fareast-font-family: ''Times New Roman''; mso-bidi-font-family: ''Times New Roman'''>We are privileged to have you as our valued customer and thank you for reposing your faith on our organisation.<o:p></o:p></span>    </p>    <p class='MsoNormal' style='MARGIN: 0in 0in 10pt; LINE-HEIGHT: normal; mso-margin-top-alt: auto; mso-margin-bottom-alt: auto'>      <b>        <span style='FONT-SIZE: 10pt; FONT-FAMILY: ''Palatino Linotype'',''serif''; mso-fareast-font-family: ''Times New Roman''; mso-bidi-font-family: ''Times New Roman'''>As per our records, your Account details are attached along with this letter:<o:p></o:p></span>      </b>    </p>    <p class='MsoNormal' style='MARGIN: 0in 0in 10pt; LINE-HEIGHT: normal; mso-margin-top-alt: auto; mso-margin-bottom-alt: auto'>      <span style='FONT-SIZE: 10pt; FONT-FAMILY: ''Palatino Linotype'',''serif''; mso-fareast-font-family: ''Times New Roman''; mso-bidi-font-family: ''Times New Roman'''>Please note that any change in the above [and also any information in the Kit] should be intimated in writing to us.<o:p></o:p></span>    </p>    <p class='MsoNormal' style='MARGIN: 0in 0in 0pt; LINE-HEIGHT: normal'>      <span style='FONT-SIZE: 10pt; COLOR: black; FONT-FAMILY: ''Palatino Linotype'',''serif''; mso-fareast-font-family: ''Times New Roman''; mso-bidi-font-family: ''Times New Roman'''>Also, please take note of following:</span>      <span style='FONT-SIZE: 12pt; FONT-FAMILY: ''Times New Roman'',''serif''; mso-fareast-font-family: ''Times New Roman'''>        <o:p>        </o:p>      </span>    </p>    <p class='MsoNormal' style='MARGIN: 0in 0in 0pt; LINE-HEIGHT: normal'>      <span style='FONT-SIZE: 12pt; FONT-FAMILY: ''Times New Roman'',''serif''; mso-fareast-font-family: ''Times New Roman'''> </span>      <span style='FONT-SIZE: 10pt; COLOR: black; FONT-FAMILY: ''Palatino Linotype'',''serif''; mso-fareast-font-family: ''Times New Roman''; mso-bidi-font-family: ''Times New Roman'''>        <o:p>        </o:p>      </span>    </p>    <p class='MsoNormal' style='MARGIN: 0in 0in 0pt 1in; TEXT-INDENT: -0.25in; LINE-HEIGHT: normal; mso-list: l0 level2 lfo1; tab-stops: list .5in 1.0in; mso-layout-grid-align: none'>      <span style='FONT-SIZE: 10pt; COLOR: black; FONT-FAMILY: Symbol; mso-fareast-font-family: Symbol; mso-bidi-font-family: Symbol'>        <span style='mso-list: Ignore'>·<span style='FONT: 7pt ''Times New Roman'''>         </span></span>      </span>      <span style='FONT-SIZE: 10pt; COLOR: black; FONT-FAMILY: ''Palatino Linotype'',''serif''; mso-fareast-font-family: ''Times New Roman''; mso-bidi-font-family: ''Times New Roman'''>Running Account authorization is voluntary, it can be revoked at any time and you can always ask for refund of excess funds or securities lying with us. <o:p></o:p></span>    </p>    <p class='MsoNormal' style='MARGIN: 0in 0in 0pt 1in; TEXT-INDENT: -0.25in; LINE-HEIGHT: normal; mso-list: l0 level2 lfo1; tab-stops: list .5in 1.0in; mso-layout-grid-align: none'>      <span style='FONT-SIZE: 10pt; COLOR: black; FONT-FAMILY: Symbol; mso-fareast-font-family: Symbol; mso-bidi-font-family: Symbol'>        <span style='mso-list: Ignore'>·<span style='FONT: 7pt ''Times New Roman'''>         </span></span>      </span>      <span style='FONT-SIZE: 10pt; COLOR: black; FONT-FAMILY: ''Palatino Linotype'',''serif''; mso-fareast-font-family: ''Times New Roman''; mso-bidi-font-family: ''Times New Roman'''>Don''t issue/receive funds/securities other than to/from Nakamichi Securities Limited. <o:p></o:p></span>    </p>    <p class='MsoNormal' style='MARGIN: 0in 0in 0pt 1in; TEXT-INDENT: -0.25in; LINE-HEIGHT: normal; mso-list: l0 level2 lfo1; tab-stops: list .5in 1.0in'>      <span style='FONT-SIZE: 10pt; COLOR: black; FONT-FAMILY: Symbol; mso-fareast-font-family: Symbol; mso-bidi-font-family: Symbol'>        <span style='mso-list: Ignore'>·<span style='FONT: 7pt ''Times New Roman'''>         </span></span>      </span>      <span style='FONT-SIZE: 10pt; COLOR: black; FONT-FAMILY: ''Palatino Linotype'',''serif''; mso-fareast-font-family: ''Times New Roman''; mso-bidi-font-family: ''Times New Roman'''>Check whether you have correctly marked the segments you wish to trade. </span>      <span style='FONT-SIZE: 10pt; COLOR: black; FONT-FAMILY: ''Palatino Linotype'',''serif''; mso-fareast-font-family: ''Times New Roman''; mso-bidi-font-family: Arial'>        <o:p>        </o:p>      </span>    </p>    <p class='MsoNormal' style='MARGIN: 0in 0in 0pt 1in; TEXT-INDENT: -0.25in; LINE-HEIGHT: normal; mso-list: l0 level2 lfo1; tab-stops: list .5in 1.0in'>      <span style='FONT-SIZE: 10pt; COLOR: black; FONT-FAMILY: Symbol; mso-fareast-font-family: Symbol; mso-bidi-font-family: Symbol; mso-bidi-font-weight: bold'>        <span style='mso-list: Ignore'>·<span style='FONT: 7pt ''Times New Roman'''>         </span></span>      </span>      <span style='FONT-SIZE: 10pt; COLOR: black; FONT-FAMILY: ''Palatino Linotype'',''serif''; mso-fareast-font-family: ''Times New Roman''; mso-bidi-font-family: Arial'>Always verify your trades with contract notes within 24 hrs and report in case it is not received by you or any discrepancy was found in the same.</span>      <span style='FONT-SIZE: 10pt; COLOR: black; FONT-FAMILY: ''Palatino Linotype'',''serif''; mso-fareast-font-family: ''Times New Roman''; mso-bidi-font-family: ''Times New Roman'''>        <b>          <o:p>          </o:p>        </b>      </span>    </p>    <p class='MsoNormal' style='MARGIN: 0in 0in 0pt 1in; TEXT-INDENT: -0.25in; LINE-HEIGHT: normal; mso-list: l0 level2 lfo1; tab-stops: list .5in 1.0in'>      <span style='FONT-SIZE: 10pt; COLOR: black; FONT-FAMILY: Symbol; mso-fareast-font-family: Symbol; mso-bidi-font-family: Symbol; mso-bidi-font-weight: bold'>        <span style='mso-list: Ignore'>·<span style='FONT: 7pt ''Times New Roman'''>         </span></span>      </span>      <span style='FONT-SIZE: 10pt; COLOR: black; FONT-FAMILY: ''Palatino Linotype'',''serif''; mso-fareast-font-family: ''Times New Roman''; mso-bidi-font-family: Arial'>Verify credit for Dividend/Corporate Actions received and report immediately if the same is not received.</span>      <span style='FONT-SIZE: 10pt; COLOR: black; FONT-FAMILY: ''Palatino Linotype'',''serif''; mso-fareast-font-family: ''Times New Roman''; mso-bidi-font-family: ''Times New Roman'''>        <b>          <o:p>          </o:p>        </b>      </span>    </p>    <p class='MsoNormal' style='MARGIN: 0in 0in 12pt 1in; TEXT-INDENT: -0.25in; LINE-HEIGHT: normal; mso-list: l0 level2 lfo1; tab-stops: list .5in 1.0in'>      <span style='FONT-SIZE: 10pt; COLOR: black; FONT-FAMILY: Symbol; mso-fareast-font-family: Symbol; mso-bidi-font-family: Symbol'>        <span style='mso-list: Ignore'>·<span style='FONT: 7pt ''Times New Roman'''>         </span></span>      </span>      <span style='FONT-SIZE: 10pt; COLOR: black; FONT-FAMILY: ''Palatino Linotype'',''serif''; mso-fareast-font-family: ''Times New Roman''; mso-bidi-font-family: Arial'>Reconcile detailed financial statement of accounts &amp; securities including margin (atleast once in a quarter) and report in case it is not received by you or any discrepancy was found in the same.<o:p></o:p></span>    </p>    <p class='MsoNormal' style='MARGIN: 0in 0in 10pt; LINE-HEIGHT: normal; mso-margin-top-alt: auto; mso-margin-bottom-alt: auto'>      <span style='FONT-SIZE: 10pt; FONT-FAMILY: ''Palatino Linotype'',''serif''; mso-fareast-font-family: ''Times New Roman''; mso-bidi-font-family: ''Times New Roman'''>For any Information/Complaint/Grivenances, please contact our Compliance officer: Ms. Garima Kanodia<br />EMail Id : <a href='mailto:investorcell@nakamichi.co.in'><span style='COLOR: blue; mso-bidi-font-size: 11.0pt'>investorcell@nakamichi.co.in </span></a>         Phones  : 91 033 40175200<br /><br />Once again, thank you for your trust in us. We are confident that we shall deliver what we promise.<br /><br />Warm Regards<o:p></o:p></span>    </p>    <p class='MsoNormal' style='MARGIN: 0in 0in 10pt; LINE-HEIGHT: normal; mso-margin-top-alt: auto; mso-margin-bottom-alt: auto'>      <span style='FONT-SIZE: 12pt; FONT-FAMILY: ''Times New Roman'',''serif''; mso-fareast-font-family: ''Times New Roman'''> <o:p></o:p></span>    </p>    <p class='MsoNormal' style='MARGIN: 0in 0in 10pt; LINE-HEIGHT: normal; mso-margin-top-alt: auto; mso-margin-bottom-alt: auto'>      <span style='FONT-SIZE: 10pt; FONT-FAMILY: ''Palatino Linotype'',''serif''; mso-fareast-font-family: ''Times New Roman''; mso-bidi-font-family: ''Times New Roman'''>________________________<br /></span>      <b>        <span style='FONT-SIZE: 10pt; FONT-FAMILY: ''Palatino Linotype'',''serif''; mso-fareast-font-family: ''Times New Roman''; mso-bidi-font-family: ''Times New Roman''; mso-bidi-font-size: 11.0pt'>Authorised Signatory</span>      </b>      <b>        <span style='FONT-SIZE: 10pt; FONT-FAMILY: ''Palatino Linotype'',''serif''; mso-fareast-font-family: ''Times New Roman''; mso-bidi-font-family: ''Times New Roman'''>          <br />        </span>      </b>      <b>        <span style='FONT-SIZE: 10pt; FONT-FAMILY: ''Palatino Linotype'',''serif''; mso-fareast-font-family: ''Times New Roman''; mso-bidi-font-family: ''Times New Roman''; mso-bidi-font-size: 11.0pt'>NAKAMICHI SECURITIES LTD</span>      </b>      <b>        <span style='FONT-SIZE: 10pt; FONT-FAMILY: ''Palatino Linotype'',''serif''; mso-fareast-font-family: ''Times New Roman''; mso-bidi-font-family: ''Times New Roman'''>          <o:p>          </o:p>        </span>      </b>    </p>    <p class='MsoNormal' style='MARGIN: 0in 0in 10pt; LINE-HEIGHT: normal; mso-margin-top-alt: auto; mso-margin-bottom-alt: auto'>      <span style='FONT-SIZE: 10pt; FONT-FAMILY: ''Palatino Linotype'',''serif''; mso-fareast-font-family: ''Times New Roman''; mso-bidi-font-family: ''Times New Roman'''> </span>      <span style='FONT-SIZE: 12pt; FONT-FAMILY: ''Times New Roman'',''serif''; mso-fareast-font-family: ''Times New Roman'''>        <o:p>        </o:p>      </span>    </p>    <p class='MsoNormal' style='MARGIN: 0in 0in 10pt; LINE-HEIGHT: normal'>      <span style='FONT-SIZE: 10pt; FONT-FAMILY: ''Palatino Linotype'',''serif''; mso-fareast-font-family: ''Times New Roman''; mso-bidi-font-family: ''Times New Roman'''>        <o:p> </o:p>      </span>    </p>    <p class='MsoNormal' style='MARGIN: 0in 0in 10pt; LINE-HEIGHT: normal; mso-margin-top-alt: auto; mso-margin-bottom-alt: auto'>      <span style='FONT-SIZE: 10pt; FONT-FAMILY: ''Palatino Linotype'',''serif''; mso-fareast-font-family: ''Times New Roman''; mso-bidi-font-family: ''Times New Roman'''> <o:p></o:p></span>    </p>    <p class='MsoNormal' style='MARGIN: 0in 0in 10pt'>      <o:p>        <font face='Calibri'> </font>      </o:p>    </p>    <p> </p>  ";
                            // Templ = "<table width='400' border='1px' cellpadding='0' cellspacing='0'><tr><td>Name</td><td>Asha kothari</td></tr><tr><td>Category</td><td>Individual - Resident</td></tr></table>";
                            //-----------
                            page = doc.Pages.AddNewPage(Winnovative.PdfCreator.PageSize.A4, new Winnovative.PdfCreator.Margins(40, 40, 10, 10), Winnovative.PdfCreator.PageOrientation.Portrait);
                            ////* font = doc.Fonts.Add(new System.Drawing.Font(new System.Drawing.FontFamily("Times New Roman"), 10, System.Drawing.GraphicsUnit.Point));
                            Templ = "<br /><br /><br />" + strCompAddrLogo + Templ;

                            //Add signature
                            if (ChkSignatory.Checked == true)
                            {
                                //Paragraph paragraph = new Paragraph();
                                string id = txtSignature_hidden.Value.ToString();
                                DataTable dtImg = oDBEngine.GetDataTable("tbl_master_document ", " top 1 * ,(select isnull(ltrim(rtrim(cnt_firstname)),'') + ' ' + isnull(ltrim(rtrim(cnt_middlename)),'') +' '+ isnull(ltrim(rtrim(cnt_lastname)),'')   from tbl_master_contact where cnt_internalid='" + txtSignature_hidden.Value + "') as SigName ", " doc_contactid='" + txtSignature_hidden.Value + "' and  doc_documentTypeId=  (select top 1 dty_id from tbl_master_documentType  where dty_documentType='Signature'  and dty_applicableFor='Employee') ");
                                string[] source = dtImg.Rows[0]["doc_source"].ToString().Split('~');
                                string imageFilePath = Server.MapPath("../Documents/") + source[1].ToString().Trim();
                                Templ = Templ + "<p><img src='" + imageFilePath + "' width='60' height='40' /></p>" + "<p>[" + dtImg.Rows[0]["SigName"].ToString() + "]</p>";
                            }

                            //Add signature

                            htmlToPdfElement = new Winnovative.PdfCreator.HtmlToPdfElement(Templ, null);
                            htmlToPdfElement.HtmlViewerWidth = 820;
                            htmlToPdfElement.FitWidth = true;
                            htmlToPdfElement.AvoidImageBreak = true;
                            htmlToPdfElement.AvoidTextBreak = true;
                            addResult = page.AddElement(htmlToPdfElement);
                        }
                    }


                    //doc.NewPage();

                    page = doc.Pages.AddNewPage();

                    if (chkHeader.Checked != true)
                    {
                        if (ChkSignatory.Checked == true)
                        {
                            //Paragraph paragraph = new Paragraph();

                            string id = txtSignature_hidden.Value.ToString();
                            DataTable dtImg = oDBEngine.GetDataTable("tbl_master_document ", " top 1 * ,(select isnull(ltrim(rtrim(cnt_firstname)),'') + ' ' + isnull(ltrim(rtrim(cnt_middlename)),'') +' '+ isnull(ltrim(rtrim(cnt_lastname)),'')   from tbl_master_contact where cnt_internalid='" + txtSignature_hidden.Value + "') as SigName ", " doc_contactid='" + txtSignature_hidden.Value + "' and  doc_documentTypeId=  (select top 1 dty_id from tbl_master_documentType  where dty_documentType='Signature'  and dty_applicableFor='Employee') ");
                            string[] source = dtImg.Rows[0]["doc_source"].ToString().Split('~');

                            string imageFilePath = Server.MapPath("../Documents/") + source[1].ToString().Trim();
                            //iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(imageFilePath);
                            //jpg.ScaleToFit(50f, 40f);
                            //jpg.SpacingBefore = 30f;
                            //jpg.SpacingAfter = 1f;
                            //jpg.Alignment = Element.ALIGN_LEFT;
                            //doc.Add(paragraph);
                            //doc.Add(jpg);
                            //Paragraph paragraph1 = new Paragraph(new Phrase(new Chunk("[" + dtImg.Rows[0]["SigName"].ToString() + "]", font9)));
                            //paragraph1.SpacingAfter = 60f;
                            //doc.Add(paragraph1);
                            //Templ = Templ + "<br /><p><img src='/assests/images/logo.jpg' /></p>";
                            strAddSignature = "<br /><p><img src='" + imageFilePath + "' width='60' height='40' /></p>" + "<p>[" + dtImg.Rows[0]["SigName"].ToString() + "]</p>";


                        }
                        //if(b==0)
                        strAddSignature = "</br>" + strCompAddrLogo + strAddSignature;

                    }
                    if (dtGeneral.Rows.Count > 0)
                    {

                        pdfFilePath = "" + dtGeneral.Rows[0]["ClientName"].ToString().Trim() + "";
                        strHeaderTable = "<br /><table width='815px' border='0' cellpadding='1px' cellspacing='0'><tr><td style='text-decoration:underline;  font-weight:bold' colspan='2'>Personal Details</td></tr>" +
                        "<tr><td style='font-family:Times New Roman; font-size:19px; width:180px'>Name &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; :</td><td style='font-family:Times New Roman; font-size:19px'>" + dtGeneral.Rows[0]["ClientName"].ToString().Trim() + "</td></tr>" +
                        "<tr><td style='font-family:Times New Roman; font-size:19px; width:180px'>UCC &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; :</td><td style='font-family:Times New Roman; font-size:19px'>" + dtGeneral.Rows[0]["ClientCode"].ToString().Trim() + "</td></tr>" +
                        "<tr><td style='font-family:Times New Roman; font-size:19px; width:180px'>Category &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; :</td><td style='font-family:Times New Roman; font-size:19px'>" + dtGeneral.Rows[0]["Category"].ToString().Trim() + "</td></tr>" +
                        "<tr><td style='font-family:Times New Roman; font-size:19px; width:180px'>Status &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; :</td><td style='font-family:Times New Roman; font-size:19px'>" + dtGeneral.Rows[0]["status"].ToString().Trim() + "</td></tr>";

                        string strAddress3 = Convert.ToString(DtAdd.Rows[0]["Addres3"]).Trim();
                        if (strAddress3 != "")
                            strAddress3 = strAddress3 + ",";

                        strHeaderTable = strHeaderTable + "<tr><td style='font-family:Times New Roman; font-size:19px; vertical-align:top'>Address &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; :</td><td style='font-family:Times New Roman; font-size:19px'>" + Convert.ToString(DtAdd.Rows[0]["Addres1"]).Trim() + ' ' + Convert.ToString(DtAdd.Rows[0]["Addres2"]).Trim() + "</br>" +
                        strAddress3 + Convert.ToString(DtAdd.Rows[0]["City"]).Trim() + "-" + Convert.ToString(DtAdd.Rows[0]["Pin"]).Trim() + "</br>" + Convert.ToString(DtAdd.Rows[0]["State"]).Trim() + "," + Convert.ToString(DtAdd.Rows[0]["Country"]).Trim() + "</td></tr>";

                        strHeaderTable = strHeaderTable + "<tr><td style='font-family:Times New Roman; font-size:19px'>Telephone Number :</td><td style='font-family:Times New Roman; font-size:19px'>" + Convert.ToString(dtGeneral.Rows[0]["TelephoneNumber"]).Trim() + "</td></tr>";
                        strHeaderTable = strHeaderTable + "<tr><td style='font-family:Times New Roman; font-size:19px'>Mobile Number &nbsp;&nbsp;&nbsp;&nbsp; :</td><td style='font-family:Times New Roman; font-size:19px'>" + Convert.ToString(dtGeneral.Rows[0]["MobNumber"]).Trim() + "</td></tr>";
                        strHeaderTable = strHeaderTable + "<tr><td style='font-family:Times New Roman; font-size:19px'>Email Id &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; :</td><td style='font-family:Times New Roman; font-size:19px'>" + Convert.ToString(dtGeneral.Rows[0]["ClientEmail"]).Trim() + "</td></tr>";
                        if (chkPrint.Checked == true)
                            strHeaderTable = strHeaderTable + "<tr><td style='font-family:Times New Roman; font-size:19px'>Printed DateTime &nbsp;&nbsp;:</td><td style='font-family:Times New Roman; font-size:19px'>" + Convert.ToString(dtGeneral.Rows[0]["PrintedDateTime"]).Trim() + "</td></tr>";

                        if (dtIntd.Rows.Count > 0)
                        {
                            strHeaderTable = strHeaderTable + "<tr><td style='font-family:Times New Roman; font-size:19px'>Introducer Name</td><td style='font-family:Times New Roman; font-size:19px'>" + Convert.ToString(dtIntd.Rows[0]["IntdName"]).Trim() + "</td></tr>";
                            strHeaderTable = strHeaderTable + "<tr><td style='font-family:Times New Roman; font-size:19px'>Introducer Code</td><td style='font-family:Times New Roman; font-size:19px'>" + Convert.ToString(dtIntd.Rows[0]["IntdUCC"]).Trim() + "</td></tr>";
                            strHeaderTable = strHeaderTable + "<tr><td style='font-family:Times New Roman; font-size:19px'>Introducer PAN</td><td style='font-family:Times New Roman; font-size:19px'>" + Convert.ToString(dtIntd.Rows[0]["IntdPAN"]).Trim() + "</td></tr>";
                        }
                        else
                            strHeaderTable = strHeaderTable + "</table>";

                    }



                    if (dtProof.Rows.Count > 0)
                    {
                        strStatutory = "<br /><table width='815px' border='1px' cellpadding='1px' cellspacing='0'><tr><td style='background-color:#C0C0C0' colspan='2'>Statutory Details</td></tr>";
                        strStatutory = strStatutory + "<tr><td style='font-family:Arial; font-size:15px; width:407px'>Document Type</td><td style='font-family:Arial; font-size:15px; width:408px'>Number</td></tr>";

                        for (int rows = 0; rows < dtProof.Rows.Count; rows++)
                        {
                            strStatutory = strStatutory + "<tr>";
                            for (int column = 0; column < dtProof.Columns.Count; column++)
                            {
                                strStatutory = strStatutory + "<td style='font-family:Times New Roman; font-size:19px'>" + Convert.ToString(dtProof.Rows[rows][column]).Trim() + "</td>";
                                //PdfPCell = new PdfPCell(new Phrase(new Chunk(dtProof.Rows[rows][column].ToString(), font9)));
                                //PdfTable.AddCell(PdfPCell);
                            }
                            strStatutory = strStatutory + "</tr>";
                        }
                        strStatutory = strStatutory + "</table>";

                    }


                    if (dtSeg.Rows.Count > 0)
                    {
                        strExchange = "<br /><table width='815px' border='1px' cellpadding='1px' cellspacing='0'><tr><td style='font-family:Times New Roman; font-size:19px;background-color:#C0C0C0' colspan='5'>Exchange Details</td></tr>";
                        strExchange = strExchange + "<tr><td style='font-family:Arial; font-size:15px; width:272px'>Segment</td><td style='font-family:Arial; font-size:15px; width:271px'>Code</td><td style='font-family:Arial; font-size:15px; width:271px'>Registration Date</td><td style='font-family:Arial; font-size:15px; width:271px'>BusinessCom. Date</td></tr>";//<td style='font-family:Arial; font-size:15px; width:271px'>Suspension Date</td>


                        for (int rows = 0; rows < dtSeg.Rows.Count; rows++)
                        {
                            strExchange = strExchange + "<tr>";
                            for (int column = 0; column < dtSeg.Columns.Count; column++)
                            {
                                strExchange = strExchange + "<td style='font-family:Arial; font-size:15px'>" + Convert.ToString(dtSeg.Rows[rows][column]).Trim() + "</td>";

                            }
                            strExchange = strExchange + "</tr>";
                        }
                        strExchange = strExchange + "</table>";

                    }




                    if (dtGroup.Rows.Count > 0)
                    {
                        strGroup = "<br /><table width='815px' border='1px' cellpadding='1px' cellspacing='0'><tr><td style='font-family:Times New Roman; font-size:19px; background-color:#C0C0C0' colspan='2'>Group Details</td></tr>";
                        strGroup = strGroup + "<tr><td style='font-family:Arial; font-size:15px; width:407px'>Group Type</td><td style='font-family:Arial; font-size:15px; width:408px'>Name</td></tr>";


                        for (int rows = 0; rows < dtGroup.Rows.Count; rows++)
                        {
                            strGroup = strGroup + "<tr>";
                            for (int column = 0; column < dtGroup.Columns.Count; column++)
                            {
                                strGroup = strGroup + "<td style='font-family:Arial; font-size:15px'>" + Convert.ToString(dtGroup.Rows[rows][column]) + "</td>";

                            }
                            strGroup = strGroup + "</tr>";
                        }
                        strGroup = strGroup + "</table>";

                    }

                    //added by krishnendu contactperson details
                    if (chkPrintCPD.Checked == true)
                    {
                        if (dtCPD.Rows.Count > 0)
                        {
                            strContactDetails = "<br/><Table width='815px' border='1px' cellpadding='1px' cellspacing='0'><tr><td style='font-family:Times New Roman;font-size:19px;background-color:#C0C0C0' colspan='2'>Contact Detils</td></tr>";
                            strContactDetails = strContactDetails + "<tr><td style='font-family:Arial;font-size:15px;width:407px'>Name</td><td style='font-family:Arial;font-size:15px;width:408px'>Relationship</td><td style='font-family:Arial;font-size:15px;width:408px'>Designation</td><td style='font-family:Arial;font-size:15px;width:408px'>Phone</td><td style='font-family:Arial;font-size:15px;width:408px'>Email</td><td style='font-family:Arial;font-size:15px;width:408px'>Status</td><td style='font-family:Arial;font-size:15px;width:408px'>PAN Number</td><td style='font-family:Arial;font-size:15px;width:408px'>Din</td></tr>";
                            for (int rows = 0; rows < dtCPD.Rows.Count; rows++)
                            {
                                strContactDetails = strContactDetails + "<tr>";
                                //for (int column = 0; column < dtCPD.Columns.Count; column++)
                                //{
                                strContactDetails = strContactDetails + "<td style='font-family:Arial; font-size:15px'>" + Convert.ToString(dtCPD.Rows[rows]["name"]) + "</td>" +
                                "<td style='font-family:Arial; font-size:15px'>" + Convert.ToString(dtCPD.Rows[rows]["cp_relationShip"]) + "</td>" +
                                "<td style='font-family:Arial; font-size:15px'>" + Convert.ToString(dtCPD.Rows[rows]["cp_designation"]) + "</td>" +
                                "<td style='font-family:Arial; font-size:15px'>" + Convert.ToString(dtCPD.Rows[rows]["Phone"]) + "</td>" +
                                "<td style='font-family:Arial; font-size:15px'>" + Convert.ToString(dtCPD.Rows[rows]["email"]) + "</td>" +
                                "<td style='font-family:Arial; font-size:15px'>" + Convert.ToString(dtCPD.Rows[rows]["cp_status"]) + "</td>" +
                                "<td style='font-family:Arial; font-size:15px'>" + Convert.ToString(dtCPD.Rows[rows]["cp_Pan"]) + "</td>" +
                                "<td style='font-family:Arial; font-size:15px'>" + Convert.ToString(dtCPD.Rows[rows]["cp_Din"]) + "</td>";


                                //}
                                strContactDetails = strContactDetails + "</tr>";
                            }
                            strContactDetails = strContactDetails + "</table>";

                        }
                    }



                    //contact person details end


                    if (rdbSegAll.Checked == false)
                    {

                        bool segfordp1 = false;
                        segfordp1 = ((HiddenField_SegmentName.Value.ToString().Contains("SPOT") || HiddenField_SegmentName.Value.ToString().Contains("COMM"))
                            && (HiddenField_SegmentName.Value.ToString().Contains("CM") || HiddenField_SegmentName.Value.ToString().Contains("FO") || HiddenField_SegmentName.Value.ToString().Contains("CDX")));
                        if (dtcm.Rows.Count > 0 && segfordp1)
                        {
                            strDPDetails = "<br /><table width='815px' border='1px' cellpadding='1px' cellspacing='0'><tr><td style='font-family:Times New Roman; font-size:19px;background-color:#C0C0C0' colspan='5'>DP Details</td></tr>";
                            strDPDetails = strDPDetails + "<tr><td style='font-family:Arial; font-size:15px; width:163px'>Category</td><td style='font-family:Arial; font-size:15px; width:163px'>POA</td><td style='font-family:Arial; font-size:15px; width:163px'>Depository ID</td><td style='font-family:Arial; font-size:15px; width:163px'>Name</td><td style='font-family:Arial; font-size:15px; width:163px'>Client ID</td></tr>";



                            for (int rows = 0; rows < dtcm.Rows.Count; rows++)
                            {
                                strDPDetails = strDPDetails + "<tr>";
                                for (int column = 0; column < dtcm.Columns.Count; column++)
                                {
                                    strDPDetails = strDPDetails + "<td style='font-family:Arial; font-size:15px'>" + Convert.ToString(dtcm.Rows[rows][column]) + "</td>";

                                }
                                strDPDetails = strDPDetails + "</tr>";
                            }
                            strDPDetails = strDPDetails + "</table>";



                        }
                        else
                        {
                            bool segfordp = false;
                            segfordp = (HiddenField_SegmentName.Value.ToString().Contains("SPOT") || HiddenField_SegmentName.Value.ToString().Contains("COMM"));
                            if (dt.Rows.Count > 0 && !segfordp)
                            {
                                strDPDetails = "<br /><table width='815px' border='1px' cellpadding='1px' cellspacing='0'><tr><td style='font-family:Times New Roman; font-size:19px;background-color:#C0C0C0' colspan='5'>DP Details</td></tr>";
                                strDPDetails = strDPDetails + "<tr><td style='font-family:Arial; font-size:15px; width:163px'>Category</td><td style='font-family:Arial; font-size:15px; width:163px'>POA</td><td style='font-family:Arial; font-size:15px; width:163px'>Depository ID</td><td style='font-family:Arial; font-size:15px; width:163px'>Name</td><td style='font-family:Arial; font-size:15px; width:163px'>Client ID</td></tr>";



                                for (int rows = 0; rows < dt.Rows.Count; rows++)
                                {
                                    strDPDetails = strDPDetails + "<tr>";
                                    for (int column = 0; column < dt.Columns.Count; column++)
                                    {

                                        strDPDetails = strDPDetails + "<td style='font-family:Arial; font-size:15px'>" + Convert.ToString(dt.Rows[rows][column]) + "</td>";

                                    }
                                    strDPDetails = strDPDetails + "</tr>";
                                }
                                strDPDetails = strDPDetails + "</table>";

                            }

                            if (dtcomm.Rows.Count > 0 && segfordp)
                            {
                                strDPDetails = "<br /><table width='815px' border='1px' cellpadding='1px' cellspacing='0'><tr><td style='font-family:Times New Roman; font-size:19px;background-color:#C0C0C0' colspan='5'>DP Details</td></tr>";
                                strDPDetails = strDPDetails + "<tr><td style='font-family:Arial; font-size:15px; width:163px'>Category</td><td style='font-family:Arial; font-size:15px; width:163px'>POA</td><td style='font-family:Arial; font-size:15px; width:163px'>Depository ID</td><td style='font-family:Arial; font-size:15px; width:163px'>Name</td><td style='font-family:Arial; font-size:15px; width:163px'>Client ID</td></tr>";

                                for (int rows = 0; rows < dtcomm.Rows.Count; rows++)
                                {
                                    strDPDetails = strDPDetails + "<tr>";
                                    for (int column = 0; column < dtcomm.Columns.Count; column++)
                                    {
                                        strDPDetails = strDPDetails + "<td style='font-family:Arial; font-size:15px'>" + Convert.ToString(dtcomm.Rows[rows][column]) + "</td>";

                                    }
                                    strDPDetails = strDPDetails + "</tr>";
                                }
                                strDPDetails = strDPDetails + "</table>";

                            }
                        }
                    }
                    else
                    {
                        strDPDetails = "<br /><table width='815px' border='1px' cellpadding='1px' cellspacing='0'><tr><td style='font-family:Times New Roman; font-size:19px;background-color:#C0C0C0' colspan='5'>DP Details</td></tr>";
                        strDPDetails = strDPDetails + "<tr><td style='font-family:Arial; font-size:15px; width:163px'>Category</td><td style='font-family:Arial; font-size:15px; width:163px'>POA</td><td style='font-family:Arial; font-size:15px; width:163px'>Depository ID</td><td style='font-family:Arial; font-size:15px; width:163px'>Name</td><td style='font-family:Arial; font-size:15px; width:163px'>Client ID</td></tr>";

                        for (int rows = 0; rows < dtcm.Rows.Count; rows++)
                        {
                            strDPDetails = strDPDetails + "<tr>";
                            for (int column = 0; column < dtcm.Columns.Count; column++)
                            {
                                strDPDetails = strDPDetails + "<td style='font-family:Arial; font-size:15px'>" + Convert.ToString(dtcm.Rows[rows][column]) + "</td>";

                            }
                            strDPDetails = strDPDetails + "</tr>";
                        }
                        strDPDetails = strDPDetails + "</table>";

                    }


                    if (dtBank.Rows.Count > 0)
                    {
                        strBankDetails = "<br /><table width='815px' border='1px' cellpadding='1px' cellspacing='0'><tr><td style='font-family:Times New Roman; font-size:19px;background-color:#C0C0C0' colspan='7'>Bank Details</td></tr>";
                        strBankDetails = strBankDetails + "<tr><td style='font-family:Arial; font-size:15px; width:117px'>Category</td><td style='font-family:Arial; font-size:15px; width:117px'>Bank Name</td><td style='font-family:Arial; font-size:15px; width:116px'>MICR No.</td><td style='font-family:Arial; font-size:15px; width:116px'>IFSC Code</td><td style='font-family:Arial; font-size:15px; width:116px'>Branch Address</td><td style='font-family:Arial; font-size:15px; width:116px'>Account Type</td><td style='font-family:Arial; font-size:15px; width:116px'>Account Number</td></tr>";

                        for (int rows = 0; rows < dtBank.Rows.Count; rows++)
                        {
                            strBankDetails = strBankDetails + "<tr>";
                            for (int column = 0; column < dtBank.Columns.Count; column++)
                            {
                                strBankDetails = strBankDetails + "<td style='font-family:Arial; font-size:15px'>" + Convert.ToString(dtBank.Rows[rows][column]) + "</td>";

                            }
                            strBankDetails = strBankDetails + "</tr>";
                        }
                        strBankDetails = strBankDetails + "</table>";


                    }
                    DataTable dtBRMain = new DataTable();
                    DataTable dtBRDetail = new DataTable();
                    DataTable dtCharge = new DataTable();
                    DataTable dtCHGRUP = new DataTable();



                    if (CheckBrkg.Checked == true)
                    {
                        if (rdbSegAll.Checked)
                        {

                            string[] seg = HiddenField_Segment.Value.ToString().Trim().Split(',');
                            int r = seg.Length;
                            for (int t = 0; t < dtSeg.Rows.Count; t++)
                            {
                                DataTable dtSegid = oDBEngine.GetDataTable("(select exch_internalid  , (select rtrim(ltrim(exh_shortName)) from  tbl_master_Exchange where exh_cntId=exch_exchid)+' '+ '-' +' '+exch_segmentId  as SegmentName from  tbl_master_companyExchange ) as T ", " *  ", " T.SegmentName like '%" + dtSeg.Rows[t]["crg_exchange"].ToString().Trim() + "%' ");
                                DataTable dtsegName = oDBEngine.GetDataTable("tbl_master_companyExchange", "(select rtrim(ltrim(exh_shortName)) from  tbl_master_Exchange where exh_cntId=exch_exchid)+' '+ '-' +' '+exch_segmentId  as SegmentName ", "exch_internalid='" + dtSegid.Rows[0]["exch_internalid"].ToString().Trim() + "'");

                                DataTable dtCH = oDBEngine.GetDataTable("trans_chargegroupmembers", "ChargeGroupMembers_ID,ChargeGroupMembers_CustomerID,ChargeGroupMembers_CompanyID,ChargeGroupMembers_SegmentID,ChargeGroupMembers_GroupType,ChargeGroupMembers_GroupCode,CONVERT(VARCHAR(10), ChargeGroupMembers_FromDate, 120) as ChargeGroupMembers_FromDate,CONVERT(VARCHAR(10), ChargeGroupMembers_UntilDate, 120) as ChargeGroupMembers_UntilDate ", "ChargeGroupMembers_CustomerId='" + DtMain.Rows[b]["cnt_internalid"].ToString().Trim() + "' and ChargeGroupMembers_SegmentID = '" + dtSegid.Rows[0]["exch_internalid"].ToString().Trim() + "' and ChargeGroupMembers_UntilDate is null");

                                if (dtCH.Rows.Count > 0)
                                {

                                    if (dtCH.Rows[0]["ChargeGroupMembers_GroupType"].ToString().Trim() == "1")
                                    {


                                        dtBRMain = oDBEngine.GetDataTable("Config_BrokerageMain", "BrokerageMain_ID,BrokerageMain_CustomerID,(select cmp_Name from tbl_master_company where cmp_internalid=BrokerageMain_CompanyID) as BrokerageMain_CompanyID,BrokerageMain_SegmentID,BrokerageMain_BrkgDecimals,case when BrokerageMain_BrkgRoundPattern=1 then 'Nearest' when BrokerageMain_BrkgRoundPattern=2 then 'Higher' when BrokerageMain_BrkgRoundPattern=3 then 'Lower' when BrokerageMain_BrkgRoundPattern=4 then 'Truncate ' when BrokerageMain_BrkgRoundPattern=5 then 'Nearest 5 Paisa' when BrokerageMain_BrkgRoundPattern=6 then 'Lower 5 Paisa' when BrokerageMain_BrkgRoundPattern=7 then 'Higher 5 Paisa' end as BrokerageMain_BrkgRoundPattern,case when BrokerageMain_AverageType=1 then 'None' when BrokerageMain_AverageType=2 then 'Order Number' when BrokerageMain_AverageType=3 then 'Instrument' when BrokerageMain_AverageType=4 then 'Similar Price' else 'None' end as BrokerageMain_AverageType ,BrokerageMain_MktDecimals,case when BrokerageMain_MktRoundPattern=1 then 'Nearest' when BrokerageMain_MktRoundPattern=2 then 'Higher' when BrokerageMain_MktRoundPattern=3 then 'Lower' when BrokerageMain_MktRoundPattern=4 then 'Truncate ' when BrokerageMain_MktRoundPattern=5 then 'Nearest 5 Paisa' when BrokerageMain_MktRoundPattern=6 then 'Lower 5 Paisa' when BrokerageMain_MktRoundPattern=7 then 'Higher 5 Paisa' end as BrokerageMain_MktRoundPattern,BrokerageMain_NetDecimals,case when BrokerageMain_NetRoundPattern=1 then 'None' when BrokerageMain_NetRoundPattern=1 then 'None' when BrokerageMain_NetRoundPattern=2 then 'Nearest Rupee' when BrokerageMain_NetRoundPattern=3 then 'Higher Rupee' when BrokerageMain_NetRoundPattern=4 then 'Lower Rupee' else 'None' end as BrokerageMain_NetRoundPattern, case  when BrokerageMain_ContractPattern=1 then 'Single' when BrokerageMain_ContractPattern=2 then 'Sharewise' when BrokerageMain_ContractPattern=3 then 'Orderwise' when BrokerageMain_ContractPattern=4 then 'Order+Sharewise' when BrokerageMain_ContractPattern=5 then 'None' else 'None' end as BrokerageMain_ContractPattern, BrokerageMain_MinDelPerShare, BrokerageMain_MaxDelPerShare, BrokerageMain_MinSqrPerShare, BrokerageMain_MaxSqrPerShare, BrokerageMain_MinDailyBrkg, BrokerageMain_MaxDailyBrkg,  BrokerageMain_MinBrkgPerOrder,BrokerageMain_MaxBrkgPerOrder, CONVERT(VARCHAR(11), BrokerageMain_FromDate , 106) as BrokerageMain_FromDate,BrokerageMain_Type  ", "BrokerageMain_CustomerID='" + DtMain.Rows[b]["cnt_internalid"].ToString().Trim() + "'  and (BrokerageMain_UntilDate is  null ) and BrokerageMain_SegmentID='" + dtSegid.Rows[0]["exch_internalid"].ToString().Trim() + "'");
                                        if (dtBRMain.Rows.Count > 0)
                                        {
                                            dtCharge = oDBEngine.GetDataTable("Config_ChargeSetup", "case when  ChargeSetup_ChargeType='CL' then 'Clearing Charges' when  ChargeSetup_ChargeType='SD' then 'Stamp Duty' when  ChargeSetup_ChargeType='DM' then 'Demat Charges' when  ChargeSetup_ChargeType='SF' then 'SEBI Fee' when  ChargeSetup_ChargeType='SX' then 'STT' when  ChargeSetup_ChargeType='ST' then 'Service Tax' when  ChargeSetup_ChargeType='TX' then 'Transaction Charge' end as ChargeSetup_ChargeType , case when ChargeSetup_ChargeBasis=1 then 'Inclusive' when ChargeSetup_ChargeBasis=2 then 'Exclusive' when ChargeSetup_ChargeBasis=3 then 'Not Applicable' end  as ChargeSetup_ChargeBasis  , (Select (isnull(ChargeGroup_Name,''))+'[' + ChargeGroup_Code+']'as d from Master_ChargeGroup WHERE  ChargeGroup_Type='2' and ChargeGroup_Code= ChargeSetup_ChargeGroup ) as  ChargeSetup_ChargeGroup ", " ChargeSetup_MainID='" + dtBRMain.Rows[0]["BrokerageMain_ID"].ToString().Trim() + "' ");
                                        }
                                    }
                                    else if (dtCH.Rows[0]["ChargeGroupMembers_GroupType"].ToString().Trim() == "2" || dtCH.Rows[0]["ChargeGroupMembers_GroupType"].ToString().Trim() == "3")
                                    {
                                        dtCHGRUP = oDBEngine.GetDataTable("master_chargegroup", "*", "chargeGroup_code='" + dtCH.Rows[0]["ChargeGroupMembers_GroupCode"].ToString().Trim() + "' and chargeGroup_Type=1");
                                        dtBRMain = oDBEngine.GetDataTable("Config_BrokerageMain", "BrokerageMain_ID,BrokerageMain_CustomerID,(select cmp_Name from tbl_master_company where cmp_internalid=BrokerageMain_CompanyID) as BrokerageMain_CompanyID,BrokerageMain_SegmentID,BrokerageMain_BrkgDecimals,case when BrokerageMain_BrkgRoundPattern=1 then 'Nearest' when BrokerageMain_BrkgRoundPattern=2 then 'Higher' when BrokerageMain_BrkgRoundPattern=3 then 'Lower' when BrokerageMain_BrkgRoundPattern=4 then 'Truncate ' when BrokerageMain_BrkgRoundPattern=5 then 'Nearest 5 Paisa' when BrokerageMain_BrkgRoundPattern=6 then 'Lower 5 Paisa' when BrokerageMain_BrkgRoundPattern=7 then 'Higher 5 Paisa' end as BrokerageMain_BrkgRoundPattern,case when BrokerageMain_AverageType=1 then 'None' when BrokerageMain_AverageType=2 then 'Order Number' when BrokerageMain_AverageType=3 then 'Instrument' when BrokerageMain_AverageType=4 then 'Similar Price' else 'None' end as BrokerageMain_AverageType ,BrokerageMain_MktDecimals,case when BrokerageMain_MktRoundPattern=1 then 'Nearest' when BrokerageMain_MktRoundPattern=2 then 'Higher' when BrokerageMain_MktRoundPattern=3 then 'Lower' when BrokerageMain_MktRoundPattern=4 then 'Truncate ' when BrokerageMain_MktRoundPattern=5 then 'Nearest 5 Paisa' when BrokerageMain_MktRoundPattern=6 then 'Lower 5 Paisa' when BrokerageMain_MktRoundPattern=7 then 'Higher 5 Paisa' end as BrokerageMain_MktRoundPattern,BrokerageMain_NetDecimals,case when BrokerageMain_NetRoundPattern=1 then 'None' when BrokerageMain_NetRoundPattern=1 then 'None' when BrokerageMain_NetRoundPattern=2 then 'Nearest Rupee' when BrokerageMain_NetRoundPattern=3 then 'Higher Rupee' when BrokerageMain_NetRoundPattern=4 then 'Lower Rupee' else 'None' end as BrokerageMain_NetRoundPattern, case  when BrokerageMain_ContractPattern=1 then 'Single' when BrokerageMain_ContractPattern=2 then 'Sharewise' when BrokerageMain_ContractPattern=3 then 'Orderwise' when BrokerageMain_ContractPattern=4 then 'Order+Sharewise' when BrokerageMain_ContractPattern=5 then 'None' else 'None' end as BrokerageMain_ContractPattern, BrokerageMain_MinDelPerShare, BrokerageMain_MaxDelPerShare, BrokerageMain_MinSqrPerShare, BrokerageMain_MaxSqrPerShare, BrokerageMain_MinDailyBrkg, BrokerageMain_MaxDailyBrkg,  BrokerageMain_MinBrkgPerOrder,BrokerageMain_MaxBrkgPerOrder,CONVERT(VARCHAR(11), BrokerageMain_FromDate , 106) as BrokerageMain_FromDate,BrokerageMain_Type   ", "BrokerageMain_CustomerID='" + dtCHGRUP.Rows[0]["ChargeGroup_Code"].ToString().Trim() + "'  and (BrokerageMain_UntilDate is  null ) and BrokerageMain_SegmentID='" + dtSegid.Rows[0]["exch_internalid"].ToString().Trim() + "'");

                                        if (dtBRMain.Rows.Count > 0)
                                        {
                                            dtCharge = oDBEngine.GetDataTable("Config_ChargeSetup", "case when  ChargeSetup_ChargeType='CL' then 'Clearing Charges' when  ChargeSetup_ChargeType='SD' then 'Stamp Duty' when  ChargeSetup_ChargeType='DM' then 'Demat Charges' when  ChargeSetup_ChargeType='SF' then 'SEBI Fee' when  ChargeSetup_ChargeType='SX' then 'STT' when  ChargeSetup_ChargeType='ST' then 'Service Tax' when  ChargeSetup_ChargeType='TX' then 'Transaction Charge' when ChargeSetup_ChargeType='CL' then 'Clearing Charge' when ChargeSetup_ChargeType='CF' then 'CNF Charge' when ChargeSetup_ChargeType='TO' then 'Total Charge' when ChargeSetup_ChargeType='DL' then 'Delivery Charge' when ChargeSetup_ChargeType='DH' then 'Delivery Holding Charge' when ChargeSetup_ChargeType='SL' then 'Sales Tax' when ChargeSetup_ChargeType='VT' then 'Vat'  end as ChargeSetup_ChargeType , case when ChargeSetup_ChargeBasis=1 then 'Inclusive' when ChargeSetup_ChargeBasis=2 then 'Exclusive' when ChargeSetup_ChargeBasis=3 then 'Not Applicable' end  as ChargeSetup_ChargeBasis  , (Select (isnull(ChargeGroup_Name,''))+'[' + ChargeGroup_Code+']'as d from Master_ChargeGroup WHERE  ChargeGroup_Type='2' and ChargeGroup_Code=ChargeSetup_ChargeGroup) as  ChargeSetup_ChargeGroup ", " ChargeSetup_MainID='" + dtBRMain.Rows[0]["BrokerageMain_ID"].ToString().Trim() + "' ");
                                        }

                                    }

                                }

                                if (dtBRMain.Rows.Count > 0)
                                {
                                    dtBRDetail = oDBEngine.GetDataTable("Config_BrokerageDetail as cbd ,Config_BrokerageMain as cbm ", "case when cbd.BrokerageDetail_MktSegment=1 then 'All' when  cbd.BrokerageDetail_MktSegment=2 then 'Rolling' when  cbd.BrokerageDetail_MktSegment=3 then 'T2T' when  cbd.BrokerageDetail_MktSegment=4 then 'Physical'  when  cbd.BrokerageDetail_MktSegment=5 then 'Institutional' when cbd.BrokerageDetail_MktSegment=6 then 'Auction' else ' ' end as BrokerageDetail_MktSegment,case when cbd.BrokerageDetail_BrkgType=1 then 'Delivery' when cbd.BrokerageDetail_BrkgType=2 then 'Square-Off' when cbd.BrokerageDetail_BrkgType=3 then 'Exercise' when cbd.BrokerageDetail_BrkgType=4 then 'Assignment' when cbd.BrokerageDetail_BrkgType=6 then 'Delivery CloseValue' else 'Final Settlement' end as BrokerageDetail_BrkgType,case when cbd.BrokerageDetail_BrkgFor=1 then 'All'  else (select Products_Name from Master_Products where Products_ID=cbd.BrokerageDetail_ProductID) end as BrokerageDetail_BrkgFor, case when  cbd.BrokerageDetail_TranType=1 then 'Purchase' when cbd.BrokerageDetail_TranType=2 then 'Sale' when cbd.BrokerageDetail_TranType=3 then 'Both' when cbd.BrokerageDetail_TranType=4 then 'FirstLeg' when cbd.BrokerageDetail_TranType=5 then 'SecondLeg' when cbd.BrokerageDetail_TranType=6 then 'HigherLeg' when cbd.BrokerageDetail_TranType=7  then 'LowerLeg' when cbd.BrokerageDetail_TranType=8 then 'Daily' when cbd.BrokerageDetail_TranType=9 then 'DailySecond' when cbd.BrokerageDetail_TranType=10 then 'Carry' else 'CarrySecond' end as BrokerageDetail_TranType,case when cbd.BrokerageDetail_InstrType=1 then 'All' when cbd.BrokerageDetail_InstrType=2 then 'Equity' when cbd.BrokerageDetail_InstrType=3 then 'Bonds'  when cbd.BrokerageDetail_InstrType=4 then 'Debt' when cbd.BrokerageDetail_InstrType=5 then 'ETFs' when cbd.BrokerageDetail_InstrType=6 then ' Equity Futures' when cbd.BrokerageDetail_InstrType=7 then 'Equity Options'  when cbd.BrokerageDetail_InstrType=8 then 'Index Futures'when cbd.BrokerageDetail_InstrType=9 then 'Index Options'when cbd.BrokerageDetail_InstrType=10 then 'All Futures'when cbd.BrokerageDetail_InstrType=11 then 'All Options' when cbd.BrokerageDetail_InstrType=12 then 'Comm Futures'when cbd.BrokerageDetail_InstrType=13 then 'Comm Options'  when cbd.BrokerageDetail_InstrType=14 then 'All Futures' else 'All Options' end as BrokerageDetail_InstrType,cast(cbd.BrokerageDetail_FlatRate  as decimal(18,2))as BrokerageDetail_FlatRate,cbd.BrokerageDetail_Rate,cast(cbd.BrokerageDetail_MinAmount  as decimal(18,2)) as BrokerageDetail_MinAmount,case when isnull(cbd.BrokerageDetail_SlabCode,'')='' then 'N/A' else cbd.BrokerageDetail_SlabCode end as BrokerageDetail_SlabCode ", "cbd.BrokerageDetail_MainID='" + dtBRMain.Rows[0]["BrokerageMain_ID"].ToString() + "' and cbd.BrokerageDetail_MainID=cbm.BrokerageMain_ID");
                                }

                                if (dtBRMain.Rows.Count > 0 && dtBRDetail.Rows.Count > 0 && dtCharge.Rows.Count > 0)
                                {


                                    if (dtCH.Rows[0]["ChargeGroupMembers_GroupType"].ToString().Trim() == "1")
                                    {

                                        strBrokHead = "<br /><table width='815px' border='1px' cellpadding='1px' cellspacing='0'><tr><td style='font-family:Times New Roman; font-size:19px;text-align: center; background-color:#C0C0C0'>" + "Brokerage Setup in " + dtsegName.Rows[0]["SegmentName"].ToString() + " Type :Specific " + "</td></tr></table>";

                                    }
                                    else if (dtCH.Rows[0]["ChargeGroupMembers_GroupType"].ToString().Trim() == "2")
                                    {

                                        strBrokHead = "<br /><table width='815px' border='1px' cellpadding='1px' cellspacing='0'><tr><td style='font-family:Times New Roman; font-size:19px;text-align: center; background-color:#C0C0C0'>" + "Brokerage Setup in " + dtsegName.Rows[0]["SegmentName"].ToString() + " Type :Scheme [Group Name:" + dtCHGRUP.Rows[0]["ChargeGroup_Name"].ToString() + "[" + dtCHGRUP.Rows[0]["ChargeGroup_Code"].ToString() + "]] " + "</td></tr></table>";
                                    }


                                }


                                if (dtBRMain.Rows.Count > 0)
                                {


                                    strBrokertageGen = "<br /><table width='815px' border='1px' cellpadding='1px' cellspacing='0'><tr><td style='font-family:Times New Roman; font-size:19px;background-color:#C0C0C0' colspan='8'>Brokerage[General]   Company:" + dtBRMain.Rows[0]["BrokerageMain_CompanyID"].ToString().Trim() + "   Date:" + dtBRMain.Rows[0]["BrokerageMain_FromDate"].ToString().Trim() + "</td></tr>";
                                    strBrokertageGen = strBrokertageGen + "<tr><td style='font-family:Arial; font-size:15px; width:101px'>Brkg Decimals</td><td style='font-family:Arial; font-size:15px; width:101px'>" + dtBRMain.Rows[0]["BrokerageMain_BrkgDecimals"].ToString().Trim() +
                                    "</td><td style='font-family:Arial; font-size:15px; width:101px'>Brkg.Rd-Off</td><td style='font-family:Arial; font-size:15px; width:102px'>" + dtBRMain.Rows[0]["BrokerageMain_BrkgRoundPattern"].ToString().Trim() +
                                    "</td><td style='font-family:Arial; font-size:15px; width:102px'>Min Daily Brkg</td><td style='font-family:Arial; font-size:15px; width:102px'>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dtBRMain.Rows[0]["BrokerageMain_MinDailyBrkg"].ToString().Trim())) +
                                    "</td><td style='font-family:Arial; font-size:15px; width:102px'>Min Sqr Brkg/Shr</td><td style='font-family:Arial; font-size:15px; width:102px'>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dtBRMain.Rows[0]["BrokerageMain_MinSqrPerShare"].ToString().Trim())) + " </td></tr>";


                                    strBrokertageGen = strBrokertageGen + "<tr><td style='font-family:Arial; font-size:15px'>Net Amt Decimal</td><td>" +
                                        dtBRMain.Rows[0]["BrokerageMain_NetDecimals"].ToString().Trim() + "</td>" +
                                    "<td style='font-family:Arial; font-size:15px'>Mkt. Rd-Off</td><td style='font-family:Arial; font-size:15px'>" + dtBRMain.Rows[0]["BrokerageMain_MktRoundPattern"].ToString().Trim() +
                                    "</td><td style='font-family:Arial; font-size:15px'>Max Daily Brkg</td><td style='font-family:Arial; font-size:15px'>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dtBRMain.Rows[0]["BrokerageMain_MaxDailyBrkg"].ToString().Trim())) +
                                    "<td style='font-family:Arial; font-size:15px'>Max Sqr Brkg/Shr</td><td style='font-family:Arial; font-size:15px'>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dtBRMain.Rows[0]["BrokerageMain_MaxSqrPerShare"].ToString().Trim())) +
                                    "</td></tr>";


                                    strBrokertageGen = strBrokertageGen + "<tr><td style='font-family:Arial; font-size:15px'>Mkt Rate Decimal</td><td>" +
                                    dtBRMain.Rows[0]["BrokerageMain_MktDecimals"].ToString().Trim() + "</td><td style='font-family:Arial; font-size:15px'>" +
                                    "Net Rd-Off</td><td style='font-family:Arial; font-size:15px'>" + dtBRMain.Rows[0]["BrokerageMain_NetRoundPattern"].ToString().Trim() +
                                    "</td><td style='font-family:Arial; font-size:15px'>Min Del Brkg/Shr</td><td style='font-family:Arial; font-size:15px'>" +
                                    oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dtBRMain.Rows[0]["BrokerageMain_MinDelPerShare"].ToString().Trim())) +
                                    "</td><td style='font-family:Arial; font-size:15px'>Min Brkg/Order</td><td style='font-family:Arial; font-size:15px'>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dtBRMain.Rows[0]["BrokerageMain_MinBrkgPerOrder"].ToString().Trim())) +
                                    "</td></tr>";

                                    strBrokertageGen = strBrokertageGen + "<tr><td style='font-family:Arial; font-size:15px'>Trd Avg.Type</td><td style='font-family:Arial; font-size:15px'>" +
                                    dtBRMain.Rows[0]["BrokerageMain_AverageType"].ToString().Trim() + "</td><td style='font-family:Arial; font-size:15px'>" +
                                    "Cont. Pattern</td><td style='font-family:Arial; font-size:15px'>" + dtBRMain.Rows[0]["BrokerageMain_ContractPattern"].ToString().Trim() +
                                    "</td><td style='font-family:Arial; font-size:15px'>Max Del Brkg/Shr</td><td style='font-family:Arial; font-size:15px'>" +
                                    oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dtBRMain.Rows[0]["BrokerageMain_MaxDelPerShare"].ToString().Trim())) +
                                    "</td><td style='font-family:Arial; font-size:15px'>Max Brkg/Order</td><td style='font-family:Arial; font-size:15px'>" +
                                    oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dtBRMain.Rows[0]["BrokerageMain_MaxBrkgPerOrder"].ToString().Trim())) +
                                    "</td></tr></table>";



                                    if (dtCharge.Rows.Count > 0)
                                    {

                                        strChargeSetUpDetails = "<br /><table width='815px' border='1px' cellpadding='1px' cellspacing='0'><tr><td style='font-family:Times New Roman; font-size:19px;background-color:#C0C0C0' colspan='3'>Charge Setup[Details]</td></tr>" +
                                        "<tr><td></td><td style='font-family:Arial; font-size:15px; width:271px'>Basis</td><td style='font-family:Arial; font-size:15px; width:271px'>Scheme</td></tr>";


                                        for (int rows = 0; rows < dtCharge.Rows.Count; rows++)
                                        {
                                            strChargeSetUpDetails = strChargeSetUpDetails + "<tr>";
                                            for (int column = 0; column < dtCharge.Columns.Count; column++)
                                            {
                                                strChargeSetUpDetails = strChargeSetUpDetails + "<td style='font-family:Arial; font-size:15px'>" + dtCharge.Rows[rows][column].ToString() + "</td>";

                                            }
                                            strChargeSetUpDetails = strChargeSetUpDetails + "</tr>";
                                        }
                                        strChargeSetUpDetails = strChargeSetUpDetails + "</table>";

                                    }
                                }

                                if (dtBRDetail.Rows.Count > 0)
                                {
                                    //PdfPTable PdfTable3 = new PdfPTable(dtBRDetail.Columns.Count);
                                    //PdfPCell PdfPCellP = null;

                                    strBrokerageSetUpDetails = "<br /><table width='815px' border='1px' cellpadding='1px' cellspacing='0'><tr><td style='font-family:Times New Roman; font-size:19px;background-color:#C0C0C0' colspan='9'>Brokerage Setup[Details]</td></tr>";
                                    strBrokerageSetUpDetails = strBrokerageSetUpDetails + "<tr><td style='font-family:Arial; font-size:15px; width:90px'>Market Segment</td>" +
                                    "<td style='font-family:Arial; font-size:15px; width:90px'>Brokerage Type</td><td style='font-family:Arial; font-size:15px; width:90px'>Brokerage For</td><td style='font-family:Arial; font-size:15px; width:90px'>Transaction Type</td>" +
                                    "<td style='font-family:Arial; font-size:15px; width:91px'>Inst. Type</td><td style='font-family:Arial; font-size:15px; width:91px'>Flat Amount</td><td style='font-family:Arial; font-size:15px; width:91px'>Rate</td><td style='font-family:Arial; font-size:15px; width:91px'>Min. Amount</td><td style='font-family:Arial; font-size:15px; width:90px'>Brokerage Slab</td></tr>";


                                    for (int rows = 0; rows < dtBRDetail.Rows.Count; rows++)
                                    {
                                        strBrokerageSetUpDetails = strBrokerageSetUpDetails + "<tr>";
                                        for (int column = 0; column < dtBRDetail.Columns.Count; column++)
                                        {
                                            strBrokerageSetUpDetails = strBrokerageSetUpDetails + "<td style='font-family:Arial; font-size:15px'>" + dtBRDetail.Rows[rows][column].ToString() + "</td>";

                                        }
                                        strBrokerageSetUpDetails = strBrokerageSetUpDetails + "</tr>";

                                    }
                                    strBrokerageSetUpDetails = strBrokerageSetUpDetails + "</table>";

                                }



                                dtCH.Clear();
                                dtCharge.Clear();
                                dtBRMain.Clear();
                                dtBRDetail.Clear();
                                dtCHGRUP.Clear();



                                strBrokerageDetailsFull = strBrokerageDetailsFull + strBrokHead + strBrokertageGen + strChargeSetUpDetails + strBrokerageSetUpDetails;
                            }


                        }
                        else
                        {


                            string[] seg = HiddenField_Segment.Value.ToString().Trim().Split(',');
                            int r = seg.Length;
                            for (int s = 0; s < r; s++)
                            {
                                DataTable dtsegName = oDBEngine.GetDataTable("tbl_master_companyExchange", "(select rtrim(ltrim(exh_shortName)) from  tbl_master_Exchange where exh_cntId=exch_exchid)+' '+ '-' +' '+exch_segmentId  as SegmentName ", "exch_internalid='" + seg[s].ToString().Trim() + "'");
                                DataTable dtCH = oDBEngine.GetDataTable("trans_chargegroupmembers", "ChargeGroupMembers_ID,ChargeGroupMembers_CustomerID,ChargeGroupMembers_CompanyID,ChargeGroupMembers_SegmentID,ChargeGroupMembers_GroupType,ChargeGroupMembers_GroupCode,CONVERT(VARCHAR(10), ChargeGroupMembers_FromDate, 120) as ChargeGroupMembers_FromDate,CONVERT(VARCHAR(10), ChargeGroupMembers_UntilDate, 120) as ChargeGroupMembers_UntilDate ", "ChargeGroupMembers_CustomerId='" + DtMain.Rows[b]["cnt_internalid"].ToString().Trim() + "' and ChargeGroupMembers_SegmentID = '" + seg[s].ToString().Trim() + "' and ChargeGroupMembers_UntilDate is null");
                                if (dtCH.Rows.Count > 0)
                                {

                                    if (dtCH.Rows[0]["ChargeGroupMembers_GroupType"].ToString().Trim() == "1")
                                    {

                                        dtBRMain = oDBEngine.GetDataTable("Config_BrokerageMain", "BrokerageMain_ID,BrokerageMain_CustomerID,(select cmp_Name from tbl_master_company where cmp_internalid=BrokerageMain_CompanyID) as BrokerageMain_CompanyID,BrokerageMain_SegmentID,BrokerageMain_BrkgDecimals,case when BrokerageMain_BrkgRoundPattern=1 then 'Nearest' when BrokerageMain_BrkgRoundPattern=2 then 'Higher' when BrokerageMain_BrkgRoundPattern=3 then 'Lower' when BrokerageMain_BrkgRoundPattern=4 then 'Truncate ' when BrokerageMain_BrkgRoundPattern=5 then 'Nearest 5 Paisa' when BrokerageMain_BrkgRoundPattern=6 then 'Lower 5 Paisa' when BrokerageMain_BrkgRoundPattern=7 then 'Higher 5 Paisa' end as BrokerageMain_BrkgRoundPattern,case when BrokerageMain_AverageType=1 then 'None' when BrokerageMain_AverageType=2 then 'Order Number' when BrokerageMain_AverageType=3 then 'Instrument' when BrokerageMain_AverageType=4 then 'Similar Price' else 'None' end as BrokerageMain_AverageType ,BrokerageMain_MktDecimals,case when BrokerageMain_MktRoundPattern=1 then 'Nearest' when BrokerageMain_MktRoundPattern=2 then 'Higher' when BrokerageMain_MktRoundPattern=3 then 'Lower' when BrokerageMain_MktRoundPattern=4 then 'Truncate ' when BrokerageMain_MktRoundPattern=5 then 'Nearest 5 Paisa' when BrokerageMain_MktRoundPattern=6 then 'Lower 5 Paisa' when BrokerageMain_MktRoundPattern=7 then 'Higher 5 Paisa' end as BrokerageMain_MktRoundPattern,BrokerageMain_NetDecimals,case when BrokerageMain_NetRoundPattern=1 then 'None' when BrokerageMain_NetRoundPattern=1 then 'None' when BrokerageMain_NetRoundPattern=2 then 'Nearest Rupee' when BrokerageMain_NetRoundPattern=3 then 'Higher Rupee' when BrokerageMain_NetRoundPattern=4 then 'Lower Rupee' else 'None' end as BrokerageMain_NetRoundPattern, case  when BrokerageMain_ContractPattern=1 then 'Single' when BrokerageMain_ContractPattern=2 then 'Sharewise' when BrokerageMain_ContractPattern=3 then 'Orderwise' when BrokerageMain_ContractPattern=4 then 'Order+Sharewise' when BrokerageMain_ContractPattern=5 then 'None' else 'None' end as BrokerageMain_ContractPattern, BrokerageMain_MinDelPerShare, BrokerageMain_MaxDelPerShare, BrokerageMain_MinSqrPerShare, BrokerageMain_MaxSqrPerShare, BrokerageMain_MinDailyBrkg, BrokerageMain_MaxDailyBrkg,  BrokerageMain_MinBrkgPerOrder,BrokerageMain_MaxBrkgPerOrder, CONVERT(VARCHAR(11), BrokerageMain_FromDate , 106) as BrokerageMain_FromDate,BrokerageMain_Type  ", "BrokerageMain_CustomerID='" + DtMain.Rows[b]["cnt_internalid"].ToString().Trim() + "'  and (BrokerageMain_UntilDate is  null ) and BrokerageMain_SegmentID='" + seg[s].ToString().Trim() + "'");
                                        if (dtBRMain.Rows.Count > 0)
                                        {
                                            dtCharge = oDBEngine.GetDataTable("Config_ChargeSetup", "case when  ChargeSetup_ChargeType='CL' then 'Clearing Charges' when  ChargeSetup_ChargeType='SD' then 'Stamp Duty' when  ChargeSetup_ChargeType='DM' then 'Demat Charges' when  ChargeSetup_ChargeType='SF' then 'SEBI Fee' when  ChargeSetup_ChargeType='SX' then 'STT' when  ChargeSetup_ChargeType='ST' then 'Service Tax' when  ChargeSetup_ChargeType='TX' then 'Transaction Charge' end as ChargeSetup_ChargeType , case when ChargeSetup_ChargeBasis=1 then 'Inclusive' when ChargeSetup_ChargeBasis=2 then 'Exclusive' when ChargeSetup_ChargeBasis=3 then 'Not Applicable' end  as ChargeSetup_ChargeBasis  , (Select (isnull(ChargeGroup_Name,''))+'[' + ChargeGroup_Code+']'as d from Master_ChargeGroup WHERE  ChargeGroup_Type='2' and ChargeGroup_Code= ChargeSetup_ChargeGroup ) as  ChargeSetup_ChargeGroup ", " ChargeSetup_MainID='" + dtBRMain.Rows[0]["BrokerageMain_ID"].ToString().Trim() + "' ");
                                        }
                                    }
                                    //else if (dtCH.Rows[0]["ChargeGroupMembers_GroupType"].ToString().Trim() == "2")
                                    else if (dtCH.Rows[0]["ChargeGroupMembers_GroupType"].ToString().Trim() == "2" || dtCH.Rows[0]["ChargeGroupMembers_GroupType"].ToString().Trim() == "3")
                                    {
                                        dtCHGRUP = oDBEngine.GetDataTable("master_chargegroup", "*", "chargeGroup_code='" + dtCH.Rows[0]["ChargeGroupMembers_GroupCode"].ToString().Trim() + "' and chargeGroup_Type=1");
                                        dtBRMain = oDBEngine.GetDataTable("Config_BrokerageMain", "BrokerageMain_ID,BrokerageMain_CustomerID,(select cmp_Name from tbl_master_company where cmp_internalid=BrokerageMain_CompanyID) as BrokerageMain_CompanyID,BrokerageMain_SegmentID,BrokerageMain_BrkgDecimals,case when BrokerageMain_BrkgRoundPattern=1 then 'Nearest' when BrokerageMain_BrkgRoundPattern=2 then 'Higher' when BrokerageMain_BrkgRoundPattern=3 then 'Lower' when BrokerageMain_BrkgRoundPattern=4 then 'Truncate ' when BrokerageMain_BrkgRoundPattern=5 then 'Nearest 5 Paisa' when BrokerageMain_BrkgRoundPattern=6 then 'Lower 5 Paisa' when BrokerageMain_BrkgRoundPattern=7 then 'Higher 5 Paisa' end as BrokerageMain_BrkgRoundPattern,case when BrokerageMain_AverageType=1 then 'None' when BrokerageMain_AverageType=2 then 'Order Number' when BrokerageMain_AverageType=3 then 'Instrument' when BrokerageMain_AverageType=4 then 'Similar Price' else 'None' end as BrokerageMain_AverageType ,BrokerageMain_MktDecimals,case when BrokerageMain_MktRoundPattern=1 then 'Nearest' when BrokerageMain_MktRoundPattern=2 then 'Higher' when BrokerageMain_MktRoundPattern=3 then 'Lower' when BrokerageMain_MktRoundPattern=4 then 'Truncate ' when BrokerageMain_MktRoundPattern=5 then 'Nearest 5 Paisa' when BrokerageMain_MktRoundPattern=6 then 'Lower 5 Paisa' when BrokerageMain_MktRoundPattern=7 then 'Higher 5 Paisa' end as BrokerageMain_MktRoundPattern,BrokerageMain_NetDecimals,case when BrokerageMain_NetRoundPattern=1 then 'None' when BrokerageMain_NetRoundPattern=1 then 'None' when BrokerageMain_NetRoundPattern=2 then 'Nearest Rupee' when BrokerageMain_NetRoundPattern=3 then 'Higher Rupee' when BrokerageMain_NetRoundPattern=4 then 'Lower Rupee' else 'None' end as BrokerageMain_NetRoundPattern, case  when BrokerageMain_ContractPattern=1 then 'Single' when BrokerageMain_ContractPattern=2 then 'Sharewise' when BrokerageMain_ContractPattern=3 then 'Orderwise' when BrokerageMain_ContractPattern=4 then 'Order+Sharewise' when BrokerageMain_ContractPattern=5 then 'None' else 'None' end as BrokerageMain_ContractPattern, BrokerageMain_MinDelPerShare, BrokerageMain_MaxDelPerShare, BrokerageMain_MinSqrPerShare, BrokerageMain_MaxSqrPerShare, BrokerageMain_MinDailyBrkg, BrokerageMain_MaxDailyBrkg,  BrokerageMain_MinBrkgPerOrder,BrokerageMain_MaxBrkgPerOrder,CONVERT(VARCHAR(11), BrokerageMain_FromDate , 106) as BrokerageMain_FromDate,BrokerageMain_Type   ", "BrokerageMain_CustomerID='" + dtCHGRUP.Rows[0]["ChargeGroup_Code"].ToString().Trim() + "'  and (BrokerageMain_UntilDate is  null ) and BrokerageMain_SegmentID='" + seg[s].ToString().Trim() + "'");
                                        if (dtBRMain.Rows.Count > 0)
                                        {
                                            dtCharge = oDBEngine.GetDataTable("Config_ChargeSetup", "case when  ChargeSetup_ChargeType='CL' then 'Clearing Charges' when  ChargeSetup_ChargeType='SD' then 'Stamp Duty' when  ChargeSetup_ChargeType='DM' then 'Demat Charges' when  ChargeSetup_ChargeType='SF' then 'SEBI Fee' when  ChargeSetup_ChargeType='SX' then 'STT' when  ChargeSetup_ChargeType='ST' then 'Service Tax' when  ChargeSetup_ChargeType='TX' then 'Transaction Charge' end as ChargeSetup_ChargeType , case when ChargeSetup_ChargeBasis=1 then 'Inclusive' when ChargeSetup_ChargeBasis=2 then 'Exclusive' when ChargeSetup_ChargeBasis=3 then 'Not Applicable' end  as ChargeSetup_ChargeBasis  , (Select (isnull(ChargeGroup_Name,''))+'[' + ChargeGroup_Code+']'as d from Master_ChargeGroup WHERE  ChargeGroup_Type='2' and ChargeGroup_Code=ChargeSetup_ChargeGroup) as  ChargeSetup_ChargeGroup ", " ChargeSetup_MainID='" + dtBRMain.Rows[0]["BrokerageMain_ID"].ToString().Trim() + "' ");
                                        }

                                    }

                                }

                                if (dtBRMain.Rows.Count > 0)
                                {
                                    dtBRDetail = oDBEngine.GetDataTable("Config_BrokerageDetail as cbd ,Config_BrokerageMain as cbm ", "case when cbd.BrokerageDetail_MktSegment=1 then 'All' when  cbd.BrokerageDetail_MktSegment=2 then 'Rolling' when  cbd.BrokerageDetail_MktSegment=3 then 'T2T' when  cbd.BrokerageDetail_MktSegment=4 then 'Physical'  when  cbd.BrokerageDetail_MktSegment=5 then 'Institutional' when cbd.BrokerageDetail_MktSegment=6 then 'Auction' else ' ' end as BrokerageDetail_MktSegment,case when cbd.BrokerageDetail_BrkgType=1 then 'Delivery' when cbd.BrokerageDetail_BrkgType=2 then 'Square-Off' when cbd.BrokerageDetail_BrkgType=3 then 'Exercise' when cbd.BrokerageDetail_BrkgType=4 then 'Assignment' when cbd.BrokerageDetail_BrkgType=6 then 'Delivery CloseValue' else 'Final Settlement' end as BrokerageDetail_BrkgType,case when cbd.BrokerageDetail_BrkgFor=1 then 'All'  else (select Products_Name from Master_Products where Products_ID=cbd.BrokerageDetail_ProductID) end as BrokerageDetail_BrkgFor, case when  cbd.BrokerageDetail_TranType=1 then 'Purchase' when cbd.BrokerageDetail_TranType=2 then 'Sale' when cbd.BrokerageDetail_TranType=3 then 'Both' when cbd.BrokerageDetail_TranType=4 then 'FirstLeg' when cbd.BrokerageDetail_TranType=5 then 'SecondLeg' when cbd.BrokerageDetail_TranType=6 then 'HigherLeg' when cbd.BrokerageDetail_TranType=7  then 'LowerLeg' when cbd.BrokerageDetail_TranType=8 then 'Daily' when cbd.BrokerageDetail_TranType=9 then 'DailySecond' when cbd.BrokerageDetail_TranType=10 then 'Carry' else 'CarrySecond' end as BrokerageDetail_TranType,case when cbd.BrokerageDetail_InstrType=1 then 'All' when cbd.BrokerageDetail_InstrType=2 then 'Equity' when cbd.BrokerageDetail_InstrType=3 then 'Bonds'  when cbd.BrokerageDetail_InstrType=4 then 'Debt' when cbd.BrokerageDetail_InstrType=5 then 'ETFs' when cbd.BrokerageDetail_InstrType=6 then ' Equity Futures' when cbd.BrokerageDetail_InstrType=7 then 'Equity Options'  when cbd.BrokerageDetail_InstrType=8 then 'Index Futures'when cbd.BrokerageDetail_InstrType=9 then 'Index Options'when cbd.BrokerageDetail_InstrType=10 then 'All Futures'when cbd.BrokerageDetail_InstrType=11 then 'All Options' when cbd.BrokerageDetail_InstrType=12 then 'Comm Futures'when cbd.BrokerageDetail_InstrType=13 then 'Comm Options'  when cbd.BrokerageDetail_InstrType=14 then 'All Futures' else 'All Options' end as BrokerageDetail_InstrType,cast(cbd.BrokerageDetail_FlatRate  as decimal(18,2))as BrokerageDetail_FlatRate,cbd.BrokerageDetail_Rate,cast(cbd.BrokerageDetail_MinAmount  as decimal(18,2)) as BrokerageDetail_MinAmount,case when isnull(cbd.BrokerageDetail_SlabCode,'')='' then 'N/A' else cbd.BrokerageDetail_SlabCode end as BrokerageDetail_SlabCode ", "cbd.BrokerageDetail_MainID='" + dtBRMain.Rows[0]["BrokerageMain_ID"].ToString() + "' and cbd.BrokerageDetail_MainID=cbm.BrokerageMain_ID");
                                }
                                if (dtBRMain.Rows.Count > 0 && dtBRDetail.Rows.Count > 0 && dtCharge.Rows.Count > 0)
                                {



                                    if (dtCH.Rows[0]["ChargeGroupMembers_GroupType"].ToString().Trim() == "1")
                                    {
                                        strBrokHead = "<table width='815px' border='1px' cellpadding='1px' cellspacing='0'><tr><td style='font-family:Times New Roman; font-size:19px;text-align: center; background-color:#C0C0C0'>" + "Brokerage Setup in " + dtsegName.Rows[0]["SegmentName"].ToString() + " Type :Specific " + "</td></tr></table>";

                                    }
                                    else if (dtCH.Rows[0]["ChargeGroupMembers_GroupType"].ToString().Trim() == "2")
                                    {
                                        strBrokHead = "<br /><table width='815px' border='1px' cellpadding='1px' cellspacing='0'><tr><td style='font-family:Times New Roman; font-size:19px;text-align: center; background-color:#C0C0C0'>" + "Brokerage Setup in " + dtsegName.Rows[0]["SegmentName"].ToString() + "  Type: <b>Scheme [Group Name:" + dtCHGRUP.Rows[0]["ChargeGroup_Name"].ToString() + "[" + dtCHGRUP.Rows[0]["ChargeGroup_Code"].ToString() + "]] " + "</td></tr></table>";


                                    }


                                }

                                if (dtBRMain.Rows.Count > 0)
                                {


                                    strBrokertageGen = "<br /><table width='815px' border='1px' cellpadding='1px' cellspacing='0'><tr><td style='font-family:Times New Roman; font-size:19px;background-color:#C0C0C0' colspan='8'>" + "Brokerage[General]   Company:" +
                                    dtBRMain.Rows[0]["BrokerageMain_CompanyID"].ToString().Trim() + "   Date:" + dtBRMain.Rows[0]["BrokerageMain_FromDate"].ToString().Trim() + "" +
                                    "</td></tr><tr><td style='font-family:Arial; font-size:15px; width:101px'>Brkg Decimals</td><td style='font-family:Arial; font-size:15px; width:101px'>" + dtBRMain.Rows[0]["BrokerageMain_BrkgDecimals"].ToString().Trim() +
                                    "</td><td style='font-family:Arial; font-size:15px; width:101px'>Brkg.Rd-Off</td><td style='font-family:Arial; font-size:15px; width:102px'>" + dtBRMain.Rows[0]["BrokerageMain_BrkgRoundPattern"].ToString().Trim() +
                                    "</td><td style='font-family:Arial; font-size:15px; width:102px'>Min Daily Brkg</td><td style='font-family:Arial; font-size:15px; width:102px'>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dtBRMain.Rows[0]["BrokerageMain_MinDailyBrkg"].ToString().Trim())) +
                                    "</td><td style='font-family:Arial; font-size:15px; width:102px'>Min Sqr Brkg/Shr</td><td style='font-family:Arial; font-size:15px; width:102px'>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dtBRMain.Rows[0]["BrokerageMain_MinSqrPerShare"].ToString().Trim())) +
                                    "</td></tr>";

                                    strBrokertageGen = strBrokertageGen + "<tr><td style='font-family:Arial; font-size:15px'>Net Amt Decimal</td><td>" +
                                    dtBRMain.Rows[0]["BrokerageMain_NetDecimals"].ToString().Trim() + "</td>" +
                                    "<td style='font-family:Arial; font-size:15px'>Mkt. Rd-Off</td><td style='font-family:Arial; font-size:15px'>" + dtBRMain.Rows[0]["BrokerageMain_MktRoundPattern"].ToString().Trim() +
                                    "</td><td style='font-family:Arial; font-size:15px'>Max Daily Brkg</td><td style='font-family:Arial; font-size:15px'>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dtBRMain.Rows[0]["BrokerageMain_MaxDailyBrkg"].ToString().Trim())) +
                                    "</td><td style='font-family:Arial; font-size:15px'>Max Sqr Brkg/Shr</td><td style='font-family:Arial; font-size:15px'>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dtBRMain.Rows[0]["BrokerageMain_MaxSqrPerShare"].ToString().Trim())) +
                                    "</td></tr>";


                                    strBrokertageGen = strBrokertageGen + "<tr><td style='font-family:Arial; font-size:15px'>Mkt Rate Decimal</td><td style='font-family:Arial; font-size:15px'>" +
                                    dtBRMain.Rows[0]["BrokerageMain_MktDecimals"].ToString().Trim() + "</td>" +
                                    "<td style='font-family:Arial; font-size:15px'>Net Rd-Off</td><td style='font-family:Arial; font-size:15px'>" + dtBRMain.Rows[0]["BrokerageMain_NetRoundPattern"].ToString().Trim() +
                                    "</td><td style='font-family:Arial; font-size:15px'>Min Del Brkg/Shr</td><td style='font-family:Arial; font-size:15px'>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dtBRMain.Rows[0]["BrokerageMain_MinDelPerShare"].ToString().Trim())) +
                                    "</td><td style='font-family:Arial; font-size:15px'>Min Brkg/Order</td><td style='font-family:Arial; font-size:15px'>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dtBRMain.Rows[0]["BrokerageMain_MinBrkgPerOrder"].ToString().Trim())) +
                                    "</td></tr>";


                                    strBrokertageGen = strBrokertageGen + "<tr><td style='font-family:Arial; font-size:15px'>Trd Avg.Type</td><td style='font-family:Arial; font-size:15px'>" +
                                    dtBRMain.Rows[0]["BrokerageMain_AverageType"].ToString().Trim() + "</td>" +
                                    "<td style='font-family:Arial; font-size:15px'>Cont. Pattern</td><td style='font-family:Arial; font-size:15px'>" + dtBRMain.Rows[0]["BrokerageMain_ContractPattern"].ToString().Trim() +
                                    "</td><td style='font-family:Arial; font-size:15px'>Max Del Brkg/Shr</td><td style='font-family:Arial; font-size:15px'>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dtBRMain.Rows[0]["BrokerageMain_MaxDelPerShare"].ToString().Trim())) +
                                    "</td><td style='font-family:Arial; font-size:15px'>Max Brkg/Order</td><td style='font-family:Arial; font-size:15px'>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dtBRMain.Rows[0]["BrokerageMain_MaxBrkgPerOrder"].ToString().Trim())) +
                                    "</td></tr>";



                                    if (dtCharge.Rows.Count > 0)
                                    {
                                        strChargeSetUpDetails = "<br /><table width='815px' border='1px' cellpadding='1px' cellspacing='0'><tr><td style='font-family:Times New Roman; font-size:19px;background-color:#C0C0C0' colspan='3'>Charge Setup[Details]</td></tr>" +
                                       "<tr><td></td><td style='font-family:Arial; font-size:15px; width:271px'>Scheme</td><td style='font-family:Arial; font-size:15px; width:271px'>Basis</td></tr>";


                                        for (int rows = 0; rows < dtCharge.Rows.Count; rows++)
                                        {
                                            strChargeSetUpDetails = strChargeSetUpDetails + "<tr>";
                                            for (int column = 0; column < dtCharge.Columns.Count; column++)
                                            {
                                                strChargeSetUpDetails = strChargeSetUpDetails + "<td style='font-family:Arial; font-size:15px'>" + dtCharge.Rows[rows][column].ToString() + "</td>";

                                            }
                                            strChargeSetUpDetails = strChargeSetUpDetails + "</tr>";
                                        }
                                        strChargeSetUpDetails = strChargeSetUpDetails + "</table>";

                                    }

                                }
                                if (dtBRDetail.Rows.Count > 0)
                                {


                                    strBrokerageSetUpDetails = "<br /><table width='815px' border='1px' cellpadding='1px' cellspacing='0'><tr><td style='font-family:Times New Roman; font-size:19px;background-color:#C0C0C0' colspan='9'>Brokerage Setup[Details]</td></tr>";
                                    strBrokerageSetUpDetails = strBrokerageSetUpDetails + "<tr><td style='font-family:Arial; font-size:15px; width:90px'>Market Segment</td>" +
                                    "<td style='font-family:Arial; font-size:15px; width:90px'>Brokerage Tye</td><td style='font-family:Arial; font-size:15px; width:90px'>Brokerage For</td><td style='font-family:Arial; font-size:15px; width:90px'>Transaction Type</td>" +
                                    "<td style='font-family:Arial; font-size:15px; width:91px'>Inst. Type</td><td style='font-family:Arial; font-size:15px; width:91px'>Flat Amount</td><td style='font-family:Arial; font-size:15px; width:91px'>Rate</td><td style='font-family:Arial; font-size:15px; width:91px'>Min. Amount</td><td style='font-family:Arial; font-size:15px; width:90px'>Brokerage Slab</td></tr>";

                                    for (int rows = 0; rows < dtBRDetail.Rows.Count; rows++)
                                    {
                                        strBrokerageSetUpDetails = strBrokerageSetUpDetails + "<tr>";
                                        for (int column = 0; column < dtBRDetail.Columns.Count; column++)
                                        {
                                            strBrokerageSetUpDetails = strBrokerageSetUpDetails + "<td style='font-family:Arial; font-size:15px'>" + dtBRDetail.Rows[rows][column].ToString() + "</td>";

                                        }
                                        strBrokerageSetUpDetails = strBrokerageSetUpDetails + "</tr>";
                                    }
                                    strBrokerageSetUpDetails = strBrokerageSetUpDetails + "</table>";


                                }

                                //if (dtCPD.Rows.Count > 0)
                                //{
                                //    strContactDetails = "<br/><Table width='815px' border='1px' cellpadding='1px' cellspacing='0'><tr><td style='font-family:Times New Roman;font-size:19px;background-color:#C0C0C0' colspan='2'>Contact Detils</td></tr>";
                                //    strContactDetails = strContactDetails + "<tr><td style='font-family:Arial;font-size:15px;width:407px'>Name</td><td style='font-family:Arial;font-size:15px;width:408px'>Relationship</td><td style='font-family:Arial;font-size:15px;width:408px'>Designation</td><td style='font-family:Arial;font-size:15px;width:408px'>Phone</td><td style='font-family:Arial;font-size:15px;width:408px'>Email</td><td style='font-family:Arial;font-size:15px;width:408px'>Status</td><td style='font-family:Arial;font-size:15px;width:408px'>PAN Number</td><td style='font-family:Arial;font-size:15px;width:408px'>Din</td></tr>";
                                //    for (int rows = 0; rows < dtCPD.Rows.Count; rows++)
                                //    {
                                //        strContactDetails = strContactDetails + "<tr>";
                                //        for (int column = 0; column < dtCPD.Columns.Count; column++)
                                //        {
                                //            strContactDetails = strContactDetails + "<td style='font-family:Arial; font-size:15px'>" + Convert.ToString(dtCPD.Rows[rows]["name"]) + "</td>" +
                                //            "<td style='font-family:Arial; font-size:15px'>" + Convert.ToString(dtCPD.Rows[rows]["cp_relationShip"]) + "</td>" +
                                //            "<td style='font-family:Arial; font-size:15px'>" + Convert.ToString(dtCPD.Rows[rows]["cp_designation"]) + "</td>" +
                                //            "<td style='font-family:Arial; font-size:15px'>" + Convert.ToString(dtCPD.Rows[rows]["Phone"]) + "</td>" +
                                //            "<td style='font-family:Arial; font-size:15px'>" + Convert.ToString(dtCPD.Rows[rows]["email"]) + "</td>" +
                                //            "<td style='font-family:Arial; font-size:15px'>" + Convert.ToString(dtCPD.Rows[rows]["cp_status"]) + "</td>" +
                                //            "<td style='font-family:Arial; font-size:15px'>" + Convert.ToString(dtCPD.Rows[rows]["cp_Pan"]) + "</td>" +
                                //            "<td style='font-family:Arial; font-size:15px'>" + Convert.ToString(dtCPD.Rows[rows]["cp_Din"]) + "</td>";
                                //        }
                                //        strContactDetails = strContactDetails + "</tr>";
                                //    }
                                //    strContactDetails = strContactDetails + "</table>";
                                //}

                                dtCH.Clear();
                                dtCharge.Clear();
                                dtBRMain.Clear();
                                dtBRDetail.Clear();
                                dtCHGRUP.Clear();
                                //dtCPD.Clear();

                                strBrokerageDetailsFull = strBrokerageDetailsFull + strBrokHead + strBrokertageGen + strChargeSetUpDetails + strBrokerageSetUpDetails;
                            }


                        }
                    }


                    // Add in the current page number using the "footer" font
                    //Paragraph para = new Paragraph("** End Of Document **", font9);
                    //para.Alignment = Element.ALIGN_CENTER;
                    //doc.Add(para);

                    // When setting the bottom margin, I just add 5 to it if we are showing page numbers


                    htmlToPdfElement = new Winnovative.PdfCreator.HtmlToPdfElement(strAddSignature + "<table width='100%'><tr><td  style='text-align: center; width:100%'>" + strHeaderTable + strStatutory + strExchange + strGroup + strDPDetails + strBankDetails + strBrokerageDetailsFull + strContactDetails + "</td></tr></table>", null);
                    addResult = page.AddElement(htmlToPdfElement);


                    if (ddllist.SelectedValue == "2")
                    {
                        string strFilePhysicalPath = Server.MapPath("../Documents/EmailAttachments");
                        if (!System.IO.Directory.Exists(strFilePhysicalPath))
                        {
                            Directory.CreateDirectory(strFilePhysicalPath);
                        }
                        pdfFilePath = (pdfFilePath.Replace("  ", " ")).Replace(" ", "_") + "_" + oDBEngine.GetDate().ToString("yyyyMMddHHmmss") + ".pdf";
                        string strFilePath = strFilePhysicalPath + "\\" + pdfFilePath;

                        doc.Save(strFilePath);
                        doc.Close();
                        string strEmailSubject = "New Account Opening Details";
                        string strEmailBody = "<table width=\"100%\"><tr><td align=\"left\" style=\"font-weight:bold;font-size:12px;\">Dear Customer," +
                            "</td></tr><tr><td align=\"left\" style=\"font-weight:bold;font-size:12px;\">Please check attached account opening details with this email." +
                            "</td></tr></table>";
                        SendMail(strEmailBody, Convert.ToString(DtMain.Rows[b]["cnt_internalid"]), strEmailSubject, "Documents/EmailAttachments/" + pdfFilePath);


                    }

                }
                //doc.Close();

                //HttpContext.Current.Response.Clear();
                //HttpContext.Current.Response.AppendHeader("content-disposition", "attachment;filename=" + pdfFilePath + ".pdf");
                //HttpContext.Current.Response.Charset = "";
                //HttpContext.Current.Response.ContentType = "application/pdf";
                //HttpContext.Current.Response.BinaryWrite(msReport.ToArray());
                //HttpContext.Current.Response.End();



                if (ddllist.SelectedValue == "1")
                {
                    doc.Save(Response, false, "ClientMasterDetails" + oDBEngine.GetDate().ToString("yyyyMMddHHmmss") + ".pdf");
                    doc.Close();
                }

                if (ddllist.SelectedValue == "2")
                {
                    Page.ClientScript.RegisterStartupScript(GetType(), "MsgEmail", "<script language='JavaScript'>alert('The Emails Are Sent !');</script>");
                }

            }
            //catch (DocumentException docEx)
            //{
            //    //handle pdf document exception if any
            //},
            catch (IOException ioEx)
            {
                // handle IO exception
            }
            catch (Exception ex)
            {
                // ahndle other exception if occurs
            }
            finally
            {
                //Close document and writer
                // doc.Close();

            }


        }


        //public void HTMLToPdf(string HTML, string FilePath)
        //{
        //    iTextSharp.text.Document document = new iTextSharp.text.Document();

        //    PdfWriter.GetInstance(document, new FileStream(Request.PhysicalApplicationPath + "\\Chap0101.pdf", FileMode.Create));
        //    document.Open();
        //    ////iTextSharp.text.Image pdfImage = iTextSharp.text.Image.GetInstance(Server.MapPath("logo.png"));

        //    ////pdfImage.ScaleToFit(100, 50);

        //    ////pdfImage.Alignment = iTextSharp.text.Image.UNDERLYING; pdfImage.SetAbsolutePosition(180, 760);

        //    ////document.Add(pdfImage);
        //    iTextSharp.text.html.simpleparser.StyleSheet styles = new iTextSharp.text.html.simpleparser.StyleSheet();
        //    iTextSharp.text.html.simpleparser.HTMLWorker hw = new iTextSharp.text.html.simpleparser.HTMLWorker(document);
        //    hw.Parse(new StringReader(HTML));
        //    document.Close();
        //    ShowPdf(Request.PhysicalApplicationPath + "\\Chap0101.pdf");
        //}
        //private void ShowPdf(string s)
        //{
        //    Response.ClearContent();
        //    Response.ClearHeaders();
        //    Response.AddHeader("Content-Disposition", "attachment;filename=" + s);
        //    Response.ContentType = "application/pdf";
        //    Response.WriteFile(s);
        //    Response.Flush();
        //    Response.Clear();
        //}
        protected void btnEmail_Click(object sender, EventArgs e)
        {
            if (ddllistType.SelectedItem.Value == "0")
            {

                HdPageNo.Value = "0";
                CreateDataTable();

                if (DtMain.Rows.Count != 0)
                {

                    //GeneratePDF();
                    GeneratePDFNew();
                }

                if (DtMain.Rows.Count == 0)
                {
                    Page.ClientScript.RegisterStartupScript(GetType(), "Message", "<script language='JavaScript'>alert('No Record To Display');</script>");
                }
                this.Page.ClientScript.RegisterStartupScript(GetType(), "height123", "<script>height();</script>");

            }
            else if (ddllistType.SelectedItem.Value == "1")
            {

                Procedure();
                print();
                this.Page.ClientScript.RegisterStartupScript(GetType(), "height123", "<script>height();</script>");
            }
        }
        protected bool SendMail(string emailbdy, string contactid, string Subject, string strFilePath)
        {

            string atchflile = "Y";
            string sPath = HttpContext.Current.Request.Url.ToString();
            string[] PageName = sPath.ToString().Split('/');
            DataTable dt = oDBEngine.GetDataTable(" tbl_trans_menu ", "mnu_id ", " mnu_menuLink like '%" + PageName[5] + "%' and mnu_segmentid='" + HttpContext.Current.Session["userlastsegment"] + "'");
            string menuId = "";
            if (dt.Rows.Count != 0)
            {
                menuId = dt.Rows[0]["mnu_id"].ToString();

            }
            else
            {
                menuId = "";
            }

            try
            {

                DataTable dt1 = oDBEngine.GetDataTable(" tbl_master_email ", "top 1 *", "eml_cntId='" + contactid + "' and eml_type='Official'");
                string mailid = "";
                string ccmail = "";
                if (dt1 != null && dt1.Rows.Count > 0)
                {

                    mailid = Convert.ToString(dt1.Rows[0]["eml_email"]);
                    // ccmail = Convert.ToString(dt1.Rows[0]["eml_ccemail"]);
                }

                if (mailid != "")
                {

                    string contactID = HttpContext.Current.Session["usercontactID"].ToString();
                    DataTable dtcmp = oDBEngine.GetDataTable(" tbl_master_company  ", "*", "cmp_id=(select emp_organization from tbl_trans_employeectc where emp_cntId='" + contactID + "' and emp_effectiveuntil is null)");
                    string cmpintid = dtcmp.Rows[0]["cmp_internalid"].ToString();
                    DataTable dtsg = oDBEngine.GetDataTable(" tbl_master_segment  ", "*", "seg_id='" + HttpContext.Current.Session["userlastsegment"].ToString() + "'");
                    string segmentname = dtsg.Rows[0]["seg_name"].ToString();

                    DataTable dtname = oDBEngine.GetDataTable(" tbl_master_Contact  ", "(isnull(RTRIM(cnt_firstName),'')+' '+isnull(RTRIM(cnt_middleName),'')+' '+isnull(RTRIM(cnt_lastName),'')+' ['+isnull(cnt_UCC,'')+']') as ClientName", "cnt_internalId='" + contactid + "' ");
                    string ClientName = dtname.Rows[0]["ClientName"].ToString();

                    string senderemail = "";
                    string[,] data = oDBEngine.GetFieldValue(" tbl_master_user,tbl_master_email ", " eml_email  AS EmployId", " user_contactId=eml_cntId and eml_type='Official' and user_id = " + HttpContext.Current.Session["userid"], 1);
                    if (data[0, 0] != "n")
                    {
                        senderemail = data[0, 0];

                    }


                    //  ###########---recipients-----------------                   

                    //DataTable dt1 = oDBEngine.GetDataTable(" tbl_master_email ", "top 1 *", "eml_cntId='" + contactid + "' and eml_type='Official'");
                    //string mailid = dt1.Rows[0]["eml_email"].ToString();
                    string contnt = "<table width=\"100%\"><tr><td align=\"left\" style=\"font-weight:bold;font-size:12px;\">Name:" + ClientName + "</td></tr><tr><td align=\"left\" style=\"font-weight:bold;font-size:12px;\">" + Subject + "</td></tr> <tr><td align=\"left\" style=\"font-weight:bold;font-size:12px;\">Segment: " + segmentname + "</td></tr><tr><td>" + emailbdy + "</td></tr></table>";
                    string InternalID = mr.InsertTransEmail(senderemail, Subject, contnt, atchflile, menuId,
                        HttpContext.Current.Session["userid"].ToString(), "N", HttpContext.Current.Session["LastCompany"].ToString(), segmentname);
                    string fValues3 = "'" + InternalID + "','" + contactid + "','" + mailid + "','TO','" + oDBEngine.GetDate().ToString() + "','" + "P" + "'";
                    oDBEngine.InsurtFieldValue("Trans_EmailRecipients", "EmailRecipients_MainID,EmailRecipients_ContactLeadID,EmailRecipients_RecipientEmailID,EmailRecipients_RecipientType,EmailRecipients_SendDateTime,EmailRecipients_Status", fValues3);
                    string strAtchFValues = InternalID.ToString() + ",'" + strFilePath + "'";
                    oDBEngine.InsurtFieldValue("Trans_EmailAttachment", "EmailAttachment_MainID,EmailAttachment_Path", strAtchFValues);

                }


            }
            catch (Exception)
            {
                return false;
            }
            return true;


        }
    }
}