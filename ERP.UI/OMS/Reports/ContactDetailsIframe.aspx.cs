using System;
using System.Data;
using BusinessLogicLayer;

namespace ERP.OMS.Reports
{
    public partial class management_ContactDetailsIframe : System.Web.UI.Page
    {
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        BusinessLogicLayer.Converter oconverter = new BusinessLogicLayer.Converter();
        string id = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            id = Request.QueryString["ID"].ToString();
            //this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>height();</script>");
            fillHTML();
        }

        public void fillHTML()
        {

            DataTable dtGeneral = oDBEngine.GetDataTable("tbl_master_contact", "top 1 isnull(ltrim(rtrim(cnt_ucc)),'') as [ClientCode]," +
                             "isnull(ltrim(rtrim(cnt_firstname)),'')+' '+ isnull(ltrim(rtrim(cnt_middlename)),'') +' '+isnull(ltrim(rtrim(cnt_lastname)),'')  as [ClientName] ," +
                             "(select  lgl_legalStatus  from tbl_master_legalstatus where lgl_id=cnt_legalStatus ) as [Category]," +
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
                             "(select top 1 isnull(REPLACE(CONVERT(VARCHAR(11), cnt_dOB, 106), ' ', '-'),' ') from tbl_master_contactRegistration where crg_cntID=cnt_internalID and crg_type='Other' and crg_registrationAuthority='ROC') as RegistrationDate", "cnt_internalID='" + id + "'");

            DataTable DtAdd = new DataTable();
            DtAdd = oDBEngine.GetDataTable("tbl_master_address", " isnull(ltrim(rtrim(add_address1)),'')  as Addres1, isnull(ltrim(rtrim(add_address2)),'')  as Addres2, isnull(ltrim(rtrim(add_address3)),'') as Addres3,(select isnull(ltrim(rtrim(city_name)),'') from tbl_master_City  where city_id =add_City) as City,(select isnull(ltrim(rtrim(state)),'') from tbl_master_state  where id= add_state) as State,(select isnull(ltrim(rtrim(cou_country)),'') from tbl_master_country  where cou_id=add_country ) as Country, isnull(ltrim(rtrim(add_pin)),'')  as Pin", " add_addressType='Registered'  and  add_cntId='" + id + "'");
            if (DtAdd.Rows.Count == 0)
            {
                DtAdd = oDBEngine.GetDataTable("tbl_master_address", "top 1 isnull(ltrim(rtrim(add_address1)),'')  as Addres1, isnull(ltrim(rtrim(add_address2)),'')  as Addres2, isnull(ltrim(rtrim(add_address3)),'') as Addres3,(select isnull(ltrim(rtrim(city_name)),'') from tbl_master_City  where city_id =add_City) as City,(select isnull(ltrim(rtrim(state)),'') from tbl_master_state  where id= add_state) as State,(select isnull(ltrim(rtrim(cou_country)),'') from tbl_master_country  where cou_id=add_country ) as Country, isnull(ltrim(rtrim(add_pin)),'')  as Pin", " add_cntId='" + id + "'");

            }

            DataTable dp = oDBEngine.GetDataTable("tbl_master_contactDPDetails", " dpd_accountType as AccountType,case when dpd_POA=1 then 'YES' else '' end as IsPOA,dpd_dpCode as dipositoryID,(select dp_dpName from tbl_master_depositoryParticipants  where left(dp_dpid,8)=left(dpd_dpCode,8)) as DipositoryName ,dpd_ClientId as  BenOwnerAccNo", "dpd_cntID='" + id + "'");
            DataTable dtBank = oDBEngine.GetDataTable("tbl_trans_contactBankDetails ,tbl_master_Bank ", "  cbd_accountCategory as Category, tbl_master_Bank.bnk_bankName AS BankName,(tbl_master_Bank.bnk_bankName + ' ' + tbl_master_Bank.bnk_branchName + ' ' + tbl_master_Bank.bnk_micrno ) AS BranchAdress,  tbl_trans_contactBankDetails.cbd_accountType  as AccountType, tbl_trans_contactBankDetails.cbd_accountNumber AS AccountNumber, tbl_master_Bank.bnk_micrno as BankMICR,tbl_master_Bank.bnk_IFSCCODE as BankIFSC", "(tbl_trans_contactBankDetails.cbd_cntId ='" + id + "') and  tbl_master_Bank.bnk_id=tbl_trans_contactBankDetails.cbd_bankCode");
            DataTable dtProof = oDBEngine.GetDataTable("tbl_master_contactRegistration", " crg_type, crg_Number,crg_place,crg_Date", "crg_cntID='" + id + "'");
            DataTable dtGroup = oDBEngine.GetDataTable(" tbl_trans_group INNER JOIN tbl_master_groupMaster ON tbl_trans_group.grp_groupMaster = tbl_master_groupMaster.gpm_id ", "tbl_trans_group.grp_id as ID,ltrim(rtrim(tbl_master_groupMaster.gpm_Description)) +'['+rtrim(ltrim(tbl_master_groupMaster.gpm_code))+']' as GroupName, tbl_master_groupMaster.gpm_Type as GroupType ", "tbl_trans_group.grp_contactId='" + id + "'");
            DataTable dtSeg = oDBEngine.GetDataTable("tbl_master_contactExchange ce ", "ce.crg_internalId as crg_internalId,ce.crg_cntID as crg_cntID,ce.crg_exchange as crg_exchange1,ce.crg_company as crg_company1,case crg_company when '0' then 'N/A' else (select cmp_name from tbl_master_company where cmp_internalId=ce.crg_company) end as crg_company,ce.crg_exchange as crg_exchange,ltrim(rtrim(ce.crg_tcode)) as crg_tcode,case when ce.crg_regisDate='1/1/1900 12:00:00 AM' then null else convert(varchar(11),ce.crg_regisDate,106) end as crg_regisDate1,case when ce.crg_regisDate='1/1/1900 12:00:00 AM' then null else convert(varchar(11),ce.crg_regisDate,106) end as crg_regisDate,case when ce.crg_businessCmmDate='1/1/1900 12:00:00 AM' then null else convert(varchar(11),ce.crg_businessCmmDate,113) end as crg_businessCmmDate1,case when ce.crg_businessCmmDate='1/1/1900 12:00:00 AM' then null else cast(ce.crg_businessCmmDate as datetime) end as crg_businessCmmDate,case when ce.crg_suspensionDate='1/1/1900 12:00:00 AM' then null else convert(varchar(11),ce.crg_suspensionDate,113) end as crg_suspensionDate1,case when ce.crg_suspensionDate='1/1/1900 12:00:00 AM' then null else cast(ce.crg_suspensionDate as datetime) end as crg_suspensionDate,ltrim(rtrim(ce.crg_reasonforSuspension)) as crg_reasonforSuspension,ce.CreateUser as CreateUser,ltrim(rtrim(ce.crg_SubBrokerFranchiseeID)) as crg_SubBrokerFranchiseeID,ltrim(rtrim(ce.crg_Dealer)) as crg_Dealer,case when ce.crg_AccountClosureDate='1/1/1900 12:00:00 AM' then null else cast(ce.crg_AccountClosureDate as datetime) end as crg_AccountClosureDate,ltrim(rtrim(ce.crg_FrontOfficeBranchCode)) as crg_FrontOfficeBranchCode,ltrim(rtrim(ce.crg_FrontOfficeGroupCode)) as crg_FrontOfficeGroupCode,ltrim(rtrim(ce.crg_ParticipantSchemeCode)) as crg_ParticipantSchemeCode,ltrim(rtrim(ce.crg_ClearingBankCode)) as crg_ClearingBankCode,ltrim(rtrim(ce.crg_SchemeCode)) as crg_SchemeCode,ltrim(rtrim(ce.crg_STTPattern)) as crg_STTPattern,(select isnull(ltrim(rtrim(cnt_firstname)),'')+' '+isnull(ltrim(rtrim(cnt_middlename)),'')+' '+isnull(ltrim(rtrim(cnt_lastname)),'')+' ['+isnull(ltrim(rtrim(cnt_shortName)),'')+']' from tbl_master_contact where cnt_internalId=ce.crg_SubBrokerFranchiseeID) as Franchisee,(select isnull(ltrim(rtrim(cnt_firstname)),'')+' '+isnull(ltrim(rtrim(cnt_middlename)),'')+' '+isnull(ltrim(rtrim(cnt_lastname)),'')+' ['+isnull(ltrim(rtrim(cnt_ucc)),'')+']' from tbl_master_contact where cnt_internalId=ce.crg_Dealer) as Dealer ", "crg_cntID='" + id + "'");


            string dspTbl = "";
            if (dtGeneral.Rows.Count == 0)
            {

                MainContainer.InnerHtml = "<table  border=\"1\"    width=\"100%\"><tr><td align=\"center\"><b>Nor Record Found:</td></tr></table>";

            }
            else
            {
                string dispTbl = "";
                dispTbl = "<table   border=\"1\"   cellpadding=\"1\" cellspacing=\"1\" width=\"100%\" style=\" font-family: verdana; font-size: 10px;border: 1px solid black;\">";
                dispTbl = dispTbl + "<tr  ><td align=\"left\"  valign=\"top\" colspan=\"2\">";
                dispTbl = dispTbl + "<table      cellpadding=\"1\" cellspacing=\"1\"  style=\" font-family: verdana; font-size: 10px;\"><tr >";
                dispTbl = dispTbl + "<td style=\"padding:5px 6px 5px 6px\">";
                dispTbl = dispTbl + "<table  border=\"1\"     cellpadding=\"1\" cellspacing=\"1\" style=\" font-family: verdana; font-size: 10px;border: 1px solid black;\" border=\"1\">";

                dispTbl = dispTbl + "<tr><td colspan=\"2\" align=\"left\"><b>Customer Details</td></td></tr>";

                dispTbl = dispTbl + "<tr  ><td ><b>Name: </td><td>&nbsp;<b>" + dtGeneral.Rows[0]["ClientName"].ToString().Trim() + "</td></tr>";
                dispTbl = dispTbl + "<tr  ><td ><b>Category: </td><td>&nbsp;" + dtGeneral.Rows[0]["Category"].ToString().Trim() + "</td></tr>";
                dispTbl = dispTbl + "<tr  ><td ><b>Address: </td><td>&nbsp;" + DtAdd.Rows[0]["Addres1"].ToString().Trim() + ' ' + DtAdd.Rows[0]["Addres2"].ToString().Trim() + ' ' + DtAdd.Rows[0]["Addres3"].ToString().Trim() + "</td></tr>";
                dispTbl = dispTbl + "<tr  ><td ><b>City: </td><td>&nbsp;" + DtAdd.Rows[0]["City"].ToString().Trim() + "</td></tr>";
                dispTbl = dispTbl + "<tr  ><td ><b>State: </td><td>&nbsp;" + DtAdd.Rows[0]["State"].ToString().Trim() + "</td></tr>";
                dispTbl = dispTbl + "<tr  ><td ><b>Country: </td><td>&nbsp;" + DtAdd.Rows[0]["Country"].ToString().Trim() + "</td></tr>";
                dispTbl = dispTbl + "<tr  ><td ><b>Pin: </td><td>&nbsp;" + DtAdd.Rows[0]["Pin"].ToString().Trim() + "</td></tr>";
                dispTbl = dispTbl + "<tr  ><td ><b>Telephone Number: </td><td>&nbsp;" + dtGeneral.Rows[0]["TelephoneNumber"].ToString().Trim() + "</td></tr>";
                dispTbl = dispTbl + "<tr  ><td ><b>Mobile Number: </td><td>&nbsp;" + dtGeneral.Rows[0]["MobNumber"].ToString().Trim() + "</td></tr>";
                dispTbl = dispTbl + "<tr  ><td ><b>Email Id: </td><td>&nbsp;" + dtGeneral.Rows[0]["ClientEmail"].ToString().Trim() + "</td></tr>";

                dispTbl = dispTbl + "</table>";
                dispTbl = dispTbl + "</td>";


                dispTbl = dispTbl + "<td  style=\"padding:5px 6px 5px 6px\" valign=\"top\">";
                //-----------Identity Details --------
                dispTbl = dispTbl + "<table  border=\"1\"     cellpadding=\"1\" cellspacing=\"1\" style=\" font-family: verdana; font-size: 10px;border: 1px solid black;\" ><tr><td colspan=\"2\" align=\"left\"><b>Statutory Details</td></tr><tr  ><td>&nbsp;Doc. Type</td><td>&nbsp;Number</td></tr>";
                for (int k = 0; k <= dtProof.Rows.Count - 1; k++)
                {
                    dispTbl = dispTbl + "<tr  ><td>&nbsp;" + dtProof.Rows[k]["crg_type"].ToString() + "</td><td>&nbsp;" + dtProof.Rows[k]["crg_Number"].ToString() + "</td></tr>";
                }
                dispTbl = dispTbl + "</table>";
                //-----------End --------
                dispTbl = dispTbl + "</td>";



                dispTbl = dispTbl + "<td style=\"padding:5px 6px 5px 6px\" valign=\"top\">";
                //-----------Unique Code--------
                dispTbl = dispTbl + "<table  border=\"1\"     cellpadding=\"1\" cellspacing=\"1\"  style=\" font-family: verdana; font-size: 10px;border: 1px solid black;\"><tr><td colspan=\"3\" align=\"left\"><b>Exchange Details</td></tr><tr  ><td>&nbsp;Segment</td><td>&nbsp;Code</td><td>&nbsp;Registration Date</td></tr>";
                for (int i = 0; i <= dtSeg.Rows.Count - 1; i++)
                {
                    dispTbl = dispTbl + "<tr  ><td>&nbsp;" + dtSeg.Rows[i]["crg_exchange"].ToString() + "</td><td>&nbsp;" + dtSeg.Rows[i]["crg_tcode"].ToString() + "</td><td>&nbsp; " + dtSeg.Rows[i]["crg_regisDate"].ToString() + "</td></tr>";
                }
                dispTbl = dispTbl + "</table>";
                //-----------End --------
                dispTbl = dispTbl + "</td>";



                dispTbl = dispTbl + "</tr>";
                dispTbl = dispTbl + "</table>";
                dispTbl = dispTbl + "</td></tr>";








                dispTbl = dispTbl + "<tr  ><td align=\"left\"  valign=\"top\" colspan=\"2\">";
                dispTbl = dispTbl + "<table      cellpadding=\"1\" cellspacing=\"1\"  style=\" font-family: verdana; font-size: 10px;\"><tr >";

                dispTbl = dispTbl + "<td style=\"padding:5px 6px 5px 6px\" valign=\"top\">";
                //-----------Group Details --------
                dispTbl = dispTbl + "<table  border=\"1\"     cellpadding=\"1\" cellspacing=\"1\" style=\" font-family: verdana; font-size: 10px;border: 1px solid black;\"><tr><td colspan=\"2\" align=\"left\"><b>Group Details</td></tr><tr  ><td>&nbsp;Group Type</td><td>&nbsp;Name</td></tr>";
                for (int j = 0; j <= dtGroup.Rows.Count - 1; j++)
                {
                    dispTbl = dispTbl + "<tr  ><td>&nbsp;" + dtGroup.Rows[j]["GroupType"].ToString() + "</td><td>&nbsp;" + dtGroup.Rows[j]["GroupName"].ToString() + "</td></tr>";
                }
                dispTbl = dispTbl + "</table>";
                //-----------End --------
                dispTbl = dispTbl + "</td>";



                dispTbl = dispTbl + "<td style=\"padding:5px 6px 5px 6px\" valign=\"top\">";
                dispTbl = dispTbl + "<table  border=\"1\"     cellpadding=\"1\" cellspacing=\"1\"  style=\" font-family: verdana; font-size: 10px;border: 1px solid black;\"><tr><td colspan=\"3\" align=\"left\"><b>DP Details</td></tr><tr  ><td>&nbsp;Category</td><td>&nbsp;Dipository ID</td><td>&nbsp;Name </td><td>&nbsp;Account ID</td><td>&nbsp;Is POA</td></tr>";
                for (int m = 0; m <= dp.Rows.Count - 1; m++)
                {
                    dispTbl = dispTbl + "<tr  ><td>&nbsp;" + dp.Rows[m]["AccountType"].ToString() + "</td><td>&nbsp;" + dp.Rows[m]["dipositoryID"].ToString() + "</td><td>&nbsp;" + dp.Rows[m]["DipositoryName"].ToString() + "</td><td>&nbsp;" + dp.Rows[m]["BenOwnerAccNo"].ToString() + "</td><td>&nbsp;" + dp.Rows[m]["IsPOA"].ToString() + "</td></tr>";
                }
                dispTbl = dispTbl + "</table>";
                dispTbl = dispTbl + "</td>";


                dispTbl = dispTbl + "</table>";
                dispTbl = dispTbl + "</td></tr>";





                dispTbl = dispTbl + "<tr  ><td align=\"left\"  valign=\"top\" colspan=\"2\">";
                dispTbl = dispTbl + "<table      cellpadding=\"1\" cellspacing=\"1\"  style=\" font-family: verdana; font-size: 10px;\"><tr >";
                dispTbl = dispTbl + "<td style=\"padding:5px 6px 5px 6px\">";
                dispTbl = dispTbl + "<table  border=\"1\"     cellpadding=\"1\" cellspacing=\"1\" style=\" font-family: verdana; font-size: 10px;border: 1px solid black;\"><tr><td colspan=\"6\" align=\"left\"><b>Bank Details</td></tr><tr  ><td>&nbsp;Category</td><td>&nbsp;Bank Name</td><td>&nbsp;MICR No.</td><td>&nbsp;IFSC Code</td><td>&nbsp;Branch Address</td><td>&nbsp;Account Type</td><td>&nbsp;Account Number</td></tr>";
                for (int l = 0; l <= dtBank.Rows.Count - 1; l++)
                {
                    dispTbl = dispTbl + "<tr  ><td>&nbsp;" + dtBank.Rows[l]["Category"].ToString() + "</td><td>&nbsp;" + dtBank.Rows[l]["BankName"].ToString() + "</td><td>&nbsp;&nbsp;" + dtBank.Rows[l]["BankMICR"].ToString() + "</td><td>&nbsp;&nbsp;" + dtBank.Rows[l]["BankIFSC"].ToString() + "</td><td>&nbsp;" + dtBank.Rows[l]["BranchAdress"].ToString() + "</td><td>&nbsp;" + dtBank.Rows[l]["AccountType"].ToString() + "</td><td>&nbsp;" + dtBank.Rows[l]["AccountNumber"].ToString() + "</td></tr>";
                }
                dispTbl = dispTbl + "</table>";
                dispTbl = dispTbl + "</td>";



                dispTbl = dispTbl + "</table>";
                dispTbl = dispTbl + "</td></tr>";


                DataTable dtBRMain = new DataTable();
                DataTable dtBRDetail = new DataTable();
                DataTable dtCharge = new DataTable();
                DataTable dtCHGRUP = new DataTable();
                int g = 0;

                if (Request.QueryString["Seg"].ToString() == "")
                {

                    //string[] seg = Request.QueryString["Seg"].ToString().ToString().Trim().Split(',');
                    //int r = seg.Length;
                    for (int t = 0; t < dtSeg.Rows.Count; t++)
                    {
                        DataTable dtSegid = oDBEngine.GetDataTable("(select exch_internalid  , (select rtrim(ltrim(exh_shortName)) from  tbl_master_Exchange where exh_cntId=exch_exchid)+' '+ '-' +' '+exch_segmentId  as SegmentName from  tbl_master_companyExchange ) as T ", " *  ", " T.SegmentName like '%" + dtSeg.Rows[t]["crg_exchange"].ToString().Trim() + "%' ");
                        DataTable dtsegName = oDBEngine.GetDataTable("tbl_master_companyExchange", "(select rtrim(ltrim(exh_shortName)) from  tbl_master_Exchange where exh_cntId=exch_exchid)+' '+ '-' +' '+exch_segmentId  as SegmentName ", "exch_internalid='" + dtSegid.Rows[0]["exch_internalid"].ToString().Trim() + "'");

                        DataTable dtCH = oDBEngine.GetDataTable("trans_chargegroupmembers", "ChargeGroupMembers_ID,ChargeGroupMembers_CustomerID,ChargeGroupMembers_CompanyID,ChargeGroupMembers_SegmentID,ChargeGroupMembers_GroupType,ChargeGroupMembers_GroupCode,CONVERT(VARCHAR(10), ChargeGroupMembers_FromDate, 120) as ChargeGroupMembers_FromDate,CONVERT(VARCHAR(10), ChargeGroupMembers_UntilDate, 120) as ChargeGroupMembers_UntilDate ", "ChargeGroupMembers_CustomerId='" + id + "' and ChargeGroupMembers_SegmentID = '" + dtSegid.Rows[0]["exch_internalid"].ToString().Trim() + "'");

                        if (dtCH.Rows.Count > 0)
                        {

                            if (dtCH.Rows[0]["ChargeGroupMembers_GroupType"].ToString().Trim() == "1")
                            {


                                dtBRMain = oDBEngine.GetDataTable("Config_BrokerageMain", "BrokerageMain_ID,BrokerageMain_CustomerID,(select cmp_Name from tbl_master_company where cmp_internalid=BrokerageMain_CompanyID) as BrokerageMain_CompanyID,BrokerageMain_SegmentID,BrokerageMain_BrkgDecimals,case when BrokerageMain_BrkgRoundPattern=1 then 'Nearest' when BrokerageMain_BrkgRoundPattern=2 then 'Higher' when BrokerageMain_BrkgRoundPattern=3 then 'Lower' when BrokerageMain_BrkgRoundPattern=4 then 'Truncate ' when BrokerageMain_BrkgRoundPattern=5 then 'Nearest 5 Paisa' when BrokerageMain_BrkgRoundPattern=6 then 'Lower 5 Paisa' when BrokerageMain_BrkgRoundPattern=7 then 'Higher 5 Paisa' end as BrokerageMain_BrkgRoundPattern,case when BrokerageMain_AverageType=1 then 'None' when BrokerageMain_AverageType=2 then 'Order Number' when BrokerageMain_AverageType=3 then 'Instrument' when BrokerageMain_AverageType=4 then 'Similar Price' else 'None' end as BrokerageMain_AverageType ,BrokerageMain_MktDecimals,case when BrokerageMain_MktRoundPattern=1 then 'Nearest' when BrokerageMain_MktRoundPattern=2 then 'Higher' when BrokerageMain_MktRoundPattern=3 then 'Lower' when BrokerageMain_MktRoundPattern=4 then 'Truncate ' when BrokerageMain_MktRoundPattern=5 then 'Nearest 5 Paisa' when BrokerageMain_MktRoundPattern=6 then 'Lower 5 Paisa' when BrokerageMain_MktRoundPattern=7 then 'Higher 5 Paisa' end as BrokerageMain_MktRoundPattern,BrokerageMain_NetDecimals,case when BrokerageMain_NetRoundPattern=1 then 'None' when BrokerageMain_NetRoundPattern=1 then 'None' when BrokerageMain_NetRoundPattern=2 then 'Nearest Rupee' when BrokerageMain_NetRoundPattern=3 then 'Higher Rupee' when BrokerageMain_NetRoundPattern=4 then 'Lower Rupee' else 'None' end as BrokerageMain_NetRoundPattern, case  when BrokerageMain_ContractPattern=1 then 'Single' when BrokerageMain_ContractPattern=2 then 'Sharewise' when BrokerageMain_ContractPattern=3 then 'Orderwise' when BrokerageMain_ContractPattern=4 then 'Order+Sharewise' when BrokerageMain_ContractPattern=5 then 'None' else 'None' end as BrokerageMain_ContractPattern, BrokerageMain_MinDelPerShare, BrokerageMain_MaxDelPerShare, BrokerageMain_MinSqrPerShare, BrokerageMain_MaxSqrPerShare, BrokerageMain_MinDailyBrkg, BrokerageMain_MaxDailyBrkg,  BrokerageMain_MinBrkgPerOrder,BrokerageMain_MaxBrkgPerOrder, CONVERT(VARCHAR(11), BrokerageMain_FromDate , 106) as BrokerageMain_FromDate,BrokerageMain_Type  ", "BrokerageMain_CustomerID='" + id + "' and  BrokerageMain_FromDate <= '" + dtCH.Rows[0]["ChargeGroupMembers_FromDate"].ToString().Trim() + "' and (BrokerageMain_UntilDate is  null or BrokerageMain_UntilDate >= '" + dtCH.Rows[0]["ChargeGroupMembers_FromDate"].ToString().Trim() + "' and BrokerageMain_SegmentID='" + dtSegid.Rows[0]["exch_internalid"].ToString().Trim() + "')");
                                if (dtBRMain.Rows.Count > 0)
                                {
                                    dtCharge = oDBEngine.GetDataTable("Config_ChargeSetup", "case when  ChargeSetup_ChargeType='SD' then 'Stamp Duty' when  ChargeSetup_ChargeType='DM' then 'Demat Charges' when  ChargeSetup_ChargeType='SF' then 'SEBI Fee' when  ChargeSetup_ChargeType='SX' then 'STT' when  ChargeSetup_ChargeType='ST' then 'Service Tax' when  ChargeSetup_ChargeType='TX' then 'Transaction Charge' end as ChargeSetup_ChargeType , case when ChargeSetup_ChargeBasis=1 then 'Inclusive' when ChargeSetup_ChargeBasis=2 then 'Exclusive' when ChargeSetup_ChargeBasis=3 then 'Not Applicable' end  as ChargeSetup_ChargeBasis  , (Select (isnull(ChargeGroup_Name,''))+'[' + ChargeGroup_Code+']'as d from Master_ChargeGroup WHERE  ChargeGroup_Type='2' and ChargeGroup_Code= ChargeSetup_ChargeGroup ) as  ChargeSetup_ChargeGroup ", " ChargeSetup_MainID='" + dtBRMain.Rows[0]["BrokerageMain_ID"].ToString().Trim() + "' ");
                                }
                            }
                            else if (dtCH.Rows[0]["ChargeGroupMembers_GroupType"].ToString().Trim() == "2")
                            {
                                dtCHGRUP = oDBEngine.GetDataTable("master_chargegroup", "*", "chargeGroup_code='" + dtCH.Rows[0]["ChargeGroupMembers_GroupCode"].ToString().Trim() + "' and chargeGroup_Type=1");
                                dtBRMain = oDBEngine.GetDataTable("Config_BrokerageMain", "BrokerageMain_ID,BrokerageMain_CustomerID,(select cmp_Name from tbl_master_company where cmp_internalid=BrokerageMain_CompanyID) as BrokerageMain_CompanyID,BrokerageMain_SegmentID,BrokerageMain_BrkgDecimals,case when BrokerageMain_BrkgRoundPattern=1 then 'Nearest' when BrokerageMain_BrkgRoundPattern=2 then 'Higher' when BrokerageMain_BrkgRoundPattern=3 then 'Lower' when BrokerageMain_BrkgRoundPattern=4 then 'Truncate ' when BrokerageMain_BrkgRoundPattern=5 then 'Nearest 5 Paisa' when BrokerageMain_BrkgRoundPattern=6 then 'Lower 5 Paisa' when BrokerageMain_BrkgRoundPattern=7 then 'Higher 5 Paisa' end as BrokerageMain_BrkgRoundPattern,case when BrokerageMain_AverageType=1 then 'None' when BrokerageMain_AverageType=2 then 'Order Number' when BrokerageMain_AverageType=3 then 'Instrument' when BrokerageMain_AverageType=4 then 'Similar Price' else 'None' end as BrokerageMain_AverageType ,BrokerageMain_MktDecimals,case when BrokerageMain_MktRoundPattern=1 then 'Nearest' when BrokerageMain_MktRoundPattern=2 then 'Higher' when BrokerageMain_MktRoundPattern=3 then 'Lower' when BrokerageMain_MktRoundPattern=4 then 'Truncate ' when BrokerageMain_MktRoundPattern=5 then 'Nearest 5 Paisa' when BrokerageMain_MktRoundPattern=6 then 'Lower 5 Paisa' when BrokerageMain_MktRoundPattern=7 then 'Higher 5 Paisa' end as BrokerageMain_MktRoundPattern,BrokerageMain_NetDecimals,case when BrokerageMain_NetRoundPattern=1 then 'None' when BrokerageMain_NetRoundPattern=1 then 'None' when BrokerageMain_NetRoundPattern=2 then 'Nearest Rupee' when BrokerageMain_NetRoundPattern=3 then 'Higher Rupee' when BrokerageMain_NetRoundPattern=4 then 'Lower Rupee' else 'None' end as BrokerageMain_NetRoundPattern, case  when BrokerageMain_ContractPattern=1 then 'Single' when BrokerageMain_ContractPattern=2 then 'Sharewise' when BrokerageMain_ContractPattern=3 then 'Orderwise' when BrokerageMain_ContractPattern=4 then 'Order+Sharewise' when BrokerageMain_ContractPattern=5 then 'None' else 'None' end as BrokerageMain_ContractPattern, BrokerageMain_MinDelPerShare, BrokerageMain_MaxDelPerShare, BrokerageMain_MinSqrPerShare, BrokerageMain_MaxSqrPerShare, BrokerageMain_MinDailyBrkg, BrokerageMain_MaxDailyBrkg,  BrokerageMain_MinBrkgPerOrder,BrokerageMain_MaxBrkgPerOrder,CONVERT(VARCHAR(11), BrokerageMain_FromDate , 106) as BrokerageMain_FromDate,BrokerageMain_Type   ", "BrokerageMain_CustomerID='" + dtCHGRUP.Rows[0]["ChargeGroup_Code"].ToString().Trim() + "' and  BrokerageMain_FromDate <= '" + dtCH.Rows[0]["ChargeGroupMembers_FromDate"].ToString().Trim() + "' and (BrokerageMain_UntilDate is  null or BrokerageMain_UntilDate >= '" + dtCH.Rows[0]["ChargeGroupMembers_FromDate"].ToString().Trim() + "' and BrokerageMain_SegmentID='" + dtSegid.Rows[0]["exch_internalid"].ToString().Trim() + "')");

                                if (dtBRMain.Rows.Count > 0)
                                {
                                    dtCharge = oDBEngine.GetDataTable("Config_ChargeSetup", "case when  ChargeSetup_ChargeType='SD' then 'Stamp Duty' when  ChargeSetup_ChargeType='DM' then 'Demat Charges' when  ChargeSetup_ChargeType='SF' then 'SEBI Fee' when  ChargeSetup_ChargeType='SX' then 'STT' when  ChargeSetup_ChargeType='ST' then 'Service Tax' when  ChargeSetup_ChargeType='TX' then 'Transaction Charge' end as ChargeSetup_ChargeType , case when ChargeSetup_ChargeBasis=1 then 'Inclusive' when ChargeSetup_ChargeBasis=2 then 'Exclusive' when ChargeSetup_ChargeBasis=3 then 'Not Applicable' end  as ChargeSetup_ChargeBasis  , (Select (isnull(ChargeGroup_Name,''))+'[' + ChargeGroup_Code+']'as d from Master_ChargeGroup WHERE  ChargeGroup_Type='2' and ChargeGroup_Code=ChargeSetup_ChargeGroup) as  ChargeSetup_ChargeGroup ", " ChargeSetup_MainID='" + dtBRMain.Rows[0]["BrokerageMain_ID"].ToString().Trim() + "' ");
                                }

                            }

                        }

                        if (dtBRMain.Rows.Count > 0)
                        {
                            dtBRDetail = oDBEngine.GetDataTable("Config_BrokerageDetail as cbd ,Config_BrokerageMain as cbm ", "cbd.BrokerageDetail_ProductID,case when cbd.BrokerageDetail_MktSegment=1 then 'All' when  cbd.BrokerageDetail_MktSegment=2 then 'Rolling' when  cbd.BrokerageDetail_MktSegment=3 then 'T2T' when  cbd.BrokerageDetail_MktSegment=4 then 'Physical'  when  cbd.BrokerageDetail_MktSegment=5 then 'Institutional' when cbd.BrokerageDetail_MktSegment=6 then 'Auction' else ' ' end as BrokerageDetail_MktSegment,cbd.BrokerageDetail_SlabCode,cbd.BrokerageDetail_ID,cast(cbd.BrokerageDetail_FlatRate  as decimal(18,2))as BrokerageDetail_FlatRate,cbd.BrokerageDetail_Rate,cast(cbd.BrokerageDetail_MinAmount  as decimal(18,2)) as BrokerageDetail_MinAmount,case when cbd.BrokerageDetail_BrkgFor=1 then 'All'  else (select Products_Name from Master_Products where Products_ID=cbd.BrokerageDetail_ProductID) end as BrokerageDetail_BrkgFor,case when cbd.BrokerageDetail_BrkgType=1 then 'Delivery' when cbd.BrokerageDetail_BrkgType=2 then 'Square-Off' when cbd.BrokerageDetail_BrkgType=3 then 'Exercise' when cbd.BrokerageDetail_BrkgType=4 then 'Assignment' when cbd.BrokerageDetail_BrkgType=6 then 'Delivery CloseValue' else 'Final Settlement' end as BrokerageDetail_BrkgType,case when  cbd.BrokerageDetail_TranType=1 then 'Purchase' when cbd.BrokerageDetail_TranType=2 then 'Sale' when cbd.BrokerageDetail_TranType=3 then 'Both' when cbd.BrokerageDetail_TranType=4 then 'FirstLeg' when cbd.BrokerageDetail_TranType=5 then 'SecondLeg' when cbd.BrokerageDetail_TranType=6 then 'HigherLeg' when cbd.BrokerageDetail_TranType=7  then 'LowerLeg' when cbd.BrokerageDetail_TranType=8 then 'Daily' when cbd.BrokerageDetail_TranType=9 then 'DailySecond' when cbd.BrokerageDetail_TranType=10 then 'Carry' else 'CarrySecond' end as BrokerageDetail_TranType,case when cbd.BrokerageDetail_InstrType=1 then 'All' when cbd.BrokerageDetail_InstrType=2 then 'Equity' when cbd.BrokerageDetail_InstrType=3 then 'Bonds'  when cbd.BrokerageDetail_InstrType=4 then 'Debt' when cbd.BrokerageDetail_InstrType=5 then 'ETFs' when cbd.BrokerageDetail_InstrType=6 then ' Equity Futures' when cbd.BrokerageDetail_InstrType=7 then 'Equity Options'  when cbd.BrokerageDetail_InstrType=8 then 'Index Futures'when cbd.BrokerageDetail_InstrType=9 then 'Index Options'when cbd.BrokerageDetail_InstrType=10 then 'All Futures'when cbd.BrokerageDetail_InstrType=11 then 'All Options' when cbd.BrokerageDetail_InstrType=12 then 'Comm Futures'when cbd.BrokerageDetail_InstrType=13 then 'Comm Options'  when cbd.BrokerageDetail_InstrType=14 then 'All Futures' else 'All Options' end as BrokerageDetail_InstrType ", "cbd.BrokerageDetail_MainID='" + dtBRMain.Rows[0]["BrokerageMain_ID"].ToString() + "' and cbd.BrokerageDetail_MainID=cbm.BrokerageMain_ID");
                        }

                        if (dtBRMain.Rows.Count > 0 && dtBRDetail.Rows.Count > 0 && dtCharge.Rows.Count > 0)
                        {


                            dispTbl = dispTbl + "<tr  ><td   align=\"center\" colspan=\"2\" style=\"padding:5px 6px 5px 6px\"><b>Brokerage Setup in " + dtsegName.Rows[0]["SegmentName"].ToString();
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

                            dispTbl = dispTbl + "<tr  ><td align=\"left\"  valign=\"top\" colspan=\"2\" style=\"padding:5px 6px 5px 6px\">";

                            dispTbl = dispTbl + "<table      cellpadding=\"1\" cellspacing=\"1\"  style=\" font-family: verdana; font-size: 10px;\"><tr >";
                            dispTbl = dispTbl + "<td valign=\"top\">";

                            dispTbl = dispTbl + "<table  border=\"1\"     cellpadding=\"1\" cellspacing=\"1\"  style=\" font-family: verdana; font-size: 10px;border: 1px solid black;\">";
                            dispTbl = dispTbl + "<tr ><td align=\"left\"  valign=\"top\" colspan=\"8\"><b>Brokerage[General] &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<b>Company:" + dtBRMain.Rows[0]["BrokerageMain_CompanyID"].ToString().Trim() + "&nbsp;&nbsp;&nbsp;&nbsp;<b>Date:" + dtBRMain.Rows[0]["BrokerageMain_FromDate"].ToString().Trim() + "</td></tr>";
                            dispTbl = dispTbl + "<tr  ><td align=\"left\"  valign=\"top\">Brkg Decimals:</td><td align=\"left\"  valign=\"top\">" + dtBRMain.Rows[0]["BrokerageMain_BrkgDecimals"].ToString().Trim() + "</td><td align=\"left\"  valign=\"top\">Brkg.Rd-Off:</td><td align=\"left\"  valign=\"top\">" + dtBRMain.Rows[0]["BrokerageMain_BrkgRoundPattern"].ToString().Trim() + "</td><td align=\"left\"  valign=\"top\">Min Daily Brkg:</td><td align=\"left\"  valign=\"top\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dtBRMain.Rows[0]["BrokerageMain_MinDailyBrkg"].ToString().Trim())) + "</td><td align=\"left\"  valign=\"top\">Min Sqr Brkg/Shr:</td><td align=\"left\"  valign=\"top\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dtBRMain.Rows[0]["BrokerageMain_MinSqrPerShare"].ToString().Trim())) + "</td></tr>";
                            dispTbl = dispTbl + "<tr  ><td align=\"left\"  valign=\"top\">Net Amt Decimal:</td><td align=\"left\"  valign=\"top\">" + dtBRMain.Rows[0]["BrokerageMain_NetDecimals"].ToString().Trim() + "</td><td align=\"left\"  valign=\"top\">Mkt. Rd-Off:</td><td align=\"left\"  valign=\"top\">" + dtBRMain.Rows[0]["BrokerageMain_MktRoundPattern"].ToString().Trim() + "</td><td align=\"left\"  valign=\"top\">Max Daily Brkg:</td><td align=\"left\"  valign=\"top\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dtBRMain.Rows[0]["BrokerageMain_MaxDailyBrkg"].ToString().Trim())) + "</td><td align=\"left\"  valign=\"top\">Max Sqr Brkg/Shr:</td><td align=\"left\"  valign=\"top\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dtBRMain.Rows[0]["BrokerageMain_MaxSqrPerShare"].ToString().Trim())) + "</td></tr>";
                            dispTbl = dispTbl + "<tr  ><td align=\"left\"  valign=\"top\">Mkt Rate Decimal:</td><td align=\"left\"  valign=\"top\">" + dtBRMain.Rows[0]["BrokerageMain_MktDecimals"].ToString().Trim() + "</td><td align=\"left\"  valign=\"top\">Net Rd-Off:</td><td align=\"left\"  valign=\"top\">" + dtBRMain.Rows[0]["BrokerageMain_NetRoundPattern"].ToString().Trim() + "</td><td align=\"left\"  valign=\"top\">Min Del Brkg/Shr:</td><td align=\"left\"  valign=\"top\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dtBRMain.Rows[0]["BrokerageMain_MinDelPerShare"].ToString().Trim())) + "</td><td align=\"left\"  valign=\"top\">Min Brkg/Order:</td><td align=\"left\"  valign=\"top\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dtBRMain.Rows[0]["BrokerageMain_MinBrkgPerOrder"].ToString().Trim())) + "</td></tr>";
                            dispTbl = dispTbl + "<tr  ><td align=\"left\"  valign=\"top\">Trd Avg.Type:</td><td align=\"left\"  valign=\"top\">" + dtBRMain.Rows[0]["BrokerageMain_AverageType"].ToString().Trim() + "</td><td align=\"left\"  valign=\"top\">Cont. Pattern:</td><td align=\"left\"  valign=\"top\">" + dtBRMain.Rows[0]["BrokerageMain_ContractPattern"].ToString().Trim() + "</td><td align=\"left\"  valign=\"top\">Max Del Brkg/Shr:</td><td align=\"left\"  valign=\"top\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dtBRMain.Rows[0]["BrokerageMain_MaxDelPerShare"].ToString().Trim())) + "</td><td align=\"left\"  valign=\"top\">Max Brkg/Order:</td><td align=\"left\"  valign=\"top\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dtBRMain.Rows[0]["BrokerageMain_MaxBrkgPerOrder"].ToString().Trim())) + "</td></tr>";
                            dispTbl = dispTbl + "</table>";

                            dispTbl = dispTbl + "</td>";
                            dispTbl = dispTbl + "<td style=\"padding:0px 6px 0px 6px\" valign=\"top\">";

                            if (dtCharge.Rows.Count > 0)
                            {
                                dispTbl = dispTbl + "<table  border=\"1\"     cellpadding=\"1\" cellspacing=\"1\" style=\" font-family: verdana; font-size: 10px;border: 1px solid black;\"><tr align=\"left\" ><td align=\"left\"  valign=\"top\" colspan=\"2\"><b>Charge Setup[Details]</td></tr><tr  ><td>&nbsp;Charges</td><td>&nbsp;Basis</td></tr>";
                                for (int p = 0; p <= dtCharge.Rows.Count - 1; p++)
                                {
                                    dispTbl = dispTbl + "<tr  ><td align=\"left\">" + dtCharge.Rows[p]["ChargeSetup_ChargeType"].ToString() + "</td><td align=\"left\">&nbsp;" + dtCharge.Rows[p]["ChargeSetup_ChargeBasis"].ToString() + "</td></tr>";
                                }
                                dispTbl = dispTbl + "</table>";


                            }

                            dispTbl = dispTbl + "</td>";
                            dispTbl = dispTbl + "</table>";



                            dispTbl = dispTbl + "</td></tr>";
                        }
                        if (dtBRDetail.Rows.Count > 0)
                        {
                            dispTbl = dispTbl + "<tr   align=\"left\"><td align=\"left\" colspan=\"2\"  valign=\"top\"  style=\"padding:5px 6px 5px 6px\">";
                            dispTbl = dispTbl + "<table  border=\"1\"     cellpadding=\"1\" cellspacing=\"1\" style=\" font-family: verdana; font-size: 10px;border: 1px solid black;\"><tr align=\"left\" ><td align=\"left\"  valign=\"top\" colspan=\"9\"><b>Brokerage Setup[Details]:</td></tr><tr  ><td>Mrkt. Segment</td><td>Brkg. Type</td><td>Brkg. For</td><td>Tran. Type</td><td>Inst. Type</td><td>Flat Amt.</td><td>Rate</td><td>Min. Amt.</td><td>Brkg. Slab</td></tr>";
                            for (int n = 0; n <= dtBRDetail.Rows.Count - 1; n++)
                            {
                                dispTbl = dispTbl + "<tr  ><td align=\"left\">&nbsp;" + dtBRDetail.Rows[n]["BrokerageDetail_MktSegment"].ToString() + "</td><td align=\"left\">&nbsp;" + dtBRDetail.Rows[n]["BrokerageDetail_BrkgType"].ToString() + "</td><td align=\"left\">&nbsp;" + dtBRDetail.Rows[n]["BrokerageDetail_BrkgFor"].ToString() + "</td><td align=\"left\">&nbsp;" + dtBRDetail.Rows[n]["BrokerageDetail_TranType"].ToString() + "</td><td align=\"left\">&nbsp;" + dtBRDetail.Rows[n]["BrokerageDetail_InstrType"].ToString() + "</td><td align=\"left\">&nbsp;" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dtBRDetail.Rows[n]["BrokerageDetail_FlatRate"].ToString())) + "</td><td align=\"left\">&nbsp;" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dtBRDetail.Rows[n]["BrokerageDetail_Rate"].ToString())) + "</td><td align=\"left\">&nbsp;" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dtBRDetail.Rows[n]["BrokerageDetail_MinAmount"].ToString())) + "</td><td align=\"left\">&nbsp;" + dtBRDetail.Rows[n]["BrokerageDetail_SlabCode"].ToString() + "</td></tr>";
                            }
                            dispTbl = dispTbl + "</table>";
                            dispTbl = dispTbl + "</td></tr>";

                        }

                        dtCH.Clear();
                        dtCharge.Clear();
                        dtBRMain.Clear();
                        dtBRDetail.Clear();
                        dtCHGRUP.Clear();


                    }


                }
                else
                {


                    string[] seg = Request.QueryString["Seg"].ToString().Trim().Split(',');
                    int r = seg.Length;
                    for (int s = 0; s < r; s++)
                    {
                        DataTable dtsegName = oDBEngine.GetDataTable("tbl_master_companyExchange", "(select rtrim(ltrim(exh_shortName)) from  tbl_master_Exchange where exh_cntId=exch_exchid)+' '+ '-' +' '+exch_segmentId  as SegmentName ", "exch_internalid='" + seg[s].ToString().Trim() + "'");
                        dispTbl = dispTbl + "<tr  ><td  colspan=\"2\" style=\"padding:5px 6px 5px 6px\"><b>Brokerage Setup in " + dtsegName.Rows[0]["SegmentName"].ToString();

                        DataTable dtCH = oDBEngine.GetDataTable("trans_chargegroupmembers", "ChargeGroupMembers_ID,ChargeGroupMembers_CustomerID,ChargeGroupMembers_CompanyID,ChargeGroupMembers_SegmentID,ChargeGroupMembers_GroupType,ChargeGroupMembers_GroupCode,CONVERT(VARCHAR(10), ChargeGroupMembers_FromDate, 120) as ChargeGroupMembers_FromDate,CONVERT(VARCHAR(10), ChargeGroupMembers_UntilDate, 120) as ChargeGroupMembers_UntilDate ", "ChargeGroupMembers_CustomerId='" + id + "' and ChargeGroupMembers_SegmentID = '" + seg[s].ToString().Trim() + "'");
                        if (dtCH.Rows.Count > 0)
                        {

                            if (dtCH.Rows[0]["ChargeGroupMembers_GroupType"].ToString().Trim() == "1")
                            {


                                dtBRMain = oDBEngine.GetDataTable("Config_BrokerageMain", "BrokerageMain_ID,BrokerageMain_CustomerID,(select cmp_Name from tbl_master_company where cmp_internalid=BrokerageMain_CompanyID) as BrokerageMain_CompanyID,BrokerageMain_SegmentID,BrokerageMain_BrkgDecimals,case when BrokerageMain_BrkgRoundPattern=1 then 'Nearest' when BrokerageMain_BrkgRoundPattern=2 then 'Higher' when BrokerageMain_BrkgRoundPattern=3 then 'Lower' when BrokerageMain_BrkgRoundPattern=4 then 'Truncate ' when BrokerageMain_BrkgRoundPattern=5 then 'Nearest 5 Paisa' when BrokerageMain_BrkgRoundPattern=6 then 'Lower 5 Paisa' when BrokerageMain_BrkgRoundPattern=7 then 'Higher 5 Paisa' end as BrokerageMain_BrkgRoundPattern,case when BrokerageMain_AverageType=1 then 'None' when BrokerageMain_AverageType=2 then 'Order Number' when BrokerageMain_AverageType=3 then 'Instrument' when BrokerageMain_AverageType=4 then 'Similar Price' else 'None' end as BrokerageMain_AverageType ,BrokerageMain_MktDecimals,case when BrokerageMain_MktRoundPattern=1 then 'Nearest' when BrokerageMain_MktRoundPattern=2 then 'Higher' when BrokerageMain_MktRoundPattern=3 then 'Lower' when BrokerageMain_MktRoundPattern=4 then 'Truncate ' when BrokerageMain_MktRoundPattern=5 then 'Nearest 5 Paisa' when BrokerageMain_MktRoundPattern=6 then 'Lower 5 Paisa' when BrokerageMain_MktRoundPattern=7 then 'Higher 5 Paisa' end as BrokerageMain_MktRoundPattern,BrokerageMain_NetDecimals,case when BrokerageMain_NetRoundPattern=1 then 'None' when BrokerageMain_NetRoundPattern=1 then 'None' when BrokerageMain_NetRoundPattern=2 then 'Nearest Rupee' when BrokerageMain_NetRoundPattern=3 then 'Higher Rupee' when BrokerageMain_NetRoundPattern=4 then 'Lower Rupee' else 'None' end as BrokerageMain_NetRoundPattern, case  when BrokerageMain_ContractPattern=1 then 'Single' when BrokerageMain_ContractPattern=2 then 'Sharewise' when BrokerageMain_ContractPattern=3 then 'Orderwise' when BrokerageMain_ContractPattern=4 then 'Order+Sharewise' when BrokerageMain_ContractPattern=5 then 'None' else 'None' end as BrokerageMain_ContractPattern, BrokerageMain_MinDelPerShare, BrokerageMain_MaxDelPerShare, BrokerageMain_MinSqrPerShare, BrokerageMain_MaxSqrPerShare, BrokerageMain_MinDailyBrkg, BrokerageMain_MaxDailyBrkg,  BrokerageMain_MinBrkgPerOrder,BrokerageMain_MaxBrkgPerOrder, CONVERT(VARCHAR(11), BrokerageMain_FromDate , 106) as BrokerageMain_FromDate,BrokerageMain_Type  ", "BrokerageMain_CustomerID='" + id + "' and  BrokerageMain_FromDate <= '" + dtCH.Rows[0]["ChargeGroupMembers_FromDate"].ToString().Trim() + "' and (BrokerageMain_UntilDate is  null or BrokerageMain_UntilDate >= '" + dtCH.Rows[0]["ChargeGroupMembers_FromDate"].ToString().Trim() + "' and BrokerageMain_SegmentID='" + seg[s].ToString().Trim() + "')");
                                if (dtBRMain.Rows.Count > 0)
                                {
                                    dtCharge = oDBEngine.GetDataTable("Config_ChargeSetup", "case when  ChargeSetup_ChargeType='SD' then 'Stamp Duty' when  ChargeSetup_ChargeType='DM' then 'Demat Charges' when  ChargeSetup_ChargeType='SF' then 'SEBI Fee' when  ChargeSetup_ChargeType='SX' then 'STT' when  ChargeSetup_ChargeType='ST' then 'Service Tax' when  ChargeSetup_ChargeType='TX' then 'Transaction Charge' end as ChargeSetup_ChargeType , case when ChargeSetup_ChargeBasis=1 then 'Inclusive' when ChargeSetup_ChargeBasis=2 then 'Exclusive' when ChargeSetup_ChargeBasis=3 then 'Not Applicable' end  as ChargeSetup_ChargeBasis  , (Select (isnull(ChargeGroup_Name,''))+'[' + ChargeGroup_Code+']'as d from Master_ChargeGroup WHERE  ChargeGroup_Type='2' and ChargeGroup_Code= ChargeSetup_ChargeGroup ) as  ChargeSetup_ChargeGroup ", " ChargeSetup_MainID='" + dtBRMain.Rows[0]["BrokerageMain_ID"].ToString().Trim() + "' ");
                                }
                            }
                            else if (dtCH.Rows[0]["ChargeGroupMembers_GroupType"].ToString().Trim() == "2")
                            {
                                dtCHGRUP = oDBEngine.GetDataTable("master_chargegroup", "*", "chargeGroup_code='" + dtCH.Rows[0]["ChargeGroupMembers_GroupCode"].ToString().Trim() + "' and chargeGroup_Type=1");
                                dtBRMain = oDBEngine.GetDataTable("Config_BrokerageMain", "BrokerageMain_ID,BrokerageMain_CustomerID,(select cmp_Name from tbl_master_company where cmp_internalid=BrokerageMain_CompanyID) as BrokerageMain_CompanyID,BrokerageMain_SegmentID,BrokerageMain_BrkgDecimals,case when BrokerageMain_BrkgRoundPattern=1 then 'Nearest' when BrokerageMain_BrkgRoundPattern=2 then 'Higher' when BrokerageMain_BrkgRoundPattern=3 then 'Lower' when BrokerageMain_BrkgRoundPattern=4 then 'Truncate ' when BrokerageMain_BrkgRoundPattern=5 then 'Nearest 5 Paisa' when BrokerageMain_BrkgRoundPattern=6 then 'Lower 5 Paisa' when BrokerageMain_BrkgRoundPattern=7 then 'Higher 5 Paisa' end as BrokerageMain_BrkgRoundPattern,case when BrokerageMain_AverageType=1 then 'None' when BrokerageMain_AverageType=2 then 'Order Number' when BrokerageMain_AverageType=3 then 'Instrument' when BrokerageMain_AverageType=4 then 'Similar Price' else 'None' end as BrokerageMain_AverageType ,BrokerageMain_MktDecimals,case when BrokerageMain_MktRoundPattern=1 then 'Nearest' when BrokerageMain_MktRoundPattern=2 then 'Higher' when BrokerageMain_MktRoundPattern=3 then 'Lower' when BrokerageMain_MktRoundPattern=4 then 'Truncate ' when BrokerageMain_MktRoundPattern=5 then 'Nearest 5 Paisa' when BrokerageMain_MktRoundPattern=6 then 'Lower 5 Paisa' when BrokerageMain_MktRoundPattern=7 then 'Higher 5 Paisa' end as BrokerageMain_MktRoundPattern,BrokerageMain_NetDecimals,case when BrokerageMain_NetRoundPattern=1 then 'None' when BrokerageMain_NetRoundPattern=1 then 'None' when BrokerageMain_NetRoundPattern=2 then 'Nearest Rupee' when BrokerageMain_NetRoundPattern=3 then 'Higher Rupee' when BrokerageMain_NetRoundPattern=4 then 'Lower Rupee' else 'None' end as BrokerageMain_NetRoundPattern, case  when BrokerageMain_ContractPattern=1 then 'Single' when BrokerageMain_ContractPattern=2 then 'Sharewise' when BrokerageMain_ContractPattern=3 then 'Orderwise' when BrokerageMain_ContractPattern=4 then 'Order+Sharewise' when BrokerageMain_ContractPattern=5 then 'None' else 'None' end as BrokerageMain_ContractPattern, BrokerageMain_MinDelPerShare, BrokerageMain_MaxDelPerShare, BrokerageMain_MinSqrPerShare, BrokerageMain_MaxSqrPerShare, BrokerageMain_MinDailyBrkg, BrokerageMain_MaxDailyBrkg,  BrokerageMain_MinBrkgPerOrder,BrokerageMain_MaxBrkgPerOrder,CONVERT(VARCHAR(11), BrokerageMain_FromDate , 106) as BrokerageMain_FromDate,BrokerageMain_Type   ", "BrokerageMain_CustomerID='" + dtCHGRUP.Rows[0]["ChargeGroup_Code"].ToString().Trim() + "' and  BrokerageMain_FromDate <= '" + dtCH.Rows[0]["ChargeGroupMembers_FromDate"].ToString().Trim() + "' and (BrokerageMain_UntilDate is  null or BrokerageMain_UntilDate >= '" + dtCH.Rows[0]["ChargeGroupMembers_FromDate"].ToString().Trim() + "' and BrokerageMain_SegmentID='" + seg[s].ToString().Trim() + "')");

                                if (dtBRMain.Rows.Count > 0)
                                {
                                    dtCharge = oDBEngine.GetDataTable("Config_ChargeSetup", "case when  ChargeSetup_ChargeType='SD' then 'Stamp Duty' when  ChargeSetup_ChargeType='DM' then 'Demat Charges' when  ChargeSetup_ChargeType='SF' then 'SEBI Fee' when  ChargeSetup_ChargeType='SX' then 'STT' when  ChargeSetup_ChargeType='ST' then 'Service Tax' when  ChargeSetup_ChargeType='TX' then 'Transaction Charge' end as ChargeSetup_ChargeType , case when ChargeSetup_ChargeBasis=1 then 'Inclusive' when ChargeSetup_ChargeBasis=2 then 'Exclusive' when ChargeSetup_ChargeBasis=3 then 'Not Applicable' end  as ChargeSetup_ChargeBasis  , (Select (isnull(ChargeGroup_Name,''))+'[' + ChargeGroup_Code+']'as d from Master_ChargeGroup WHERE  ChargeGroup_Type='2' and ChargeGroup_Code=ChargeSetup_ChargeGroup) as  ChargeSetup_ChargeGroup ", " ChargeSetup_MainID='" + dtBRMain.Rows[0]["BrokerageMain_ID"].ToString().Trim() + "' ");
                                }

                            }

                        }

                        if (dtBRMain.Rows.Count > 0)
                        {
                            dtBRDetail = oDBEngine.GetDataTable("Config_BrokerageDetail as cbd ,Config_BrokerageMain as cbm ", "cbd.BrokerageDetail_ProductID,case when cbd.BrokerageDetail_MktSegment=1 then 'All' when  cbd.BrokerageDetail_MktSegment=2 then 'Rolling' when  cbd.BrokerageDetail_MktSegment=3 then 'T2T' when  cbd.BrokerageDetail_MktSegment=4 then 'Physical'  when  cbd.BrokerageDetail_MktSegment=5 then 'Institutional' when cbd.BrokerageDetail_MktSegment=6 then 'Auction' else ' ' end as BrokerageDetail_MktSegment,cbd.BrokerageDetail_SlabCode,cbd.BrokerageDetail_ID,cast(cbd.BrokerageDetail_FlatRate  as decimal(18,2))as BrokerageDetail_FlatRate,cbd.BrokerageDetail_Rate,cast(cbd.BrokerageDetail_MinAmount  as decimal(18,2)) as BrokerageDetail_MinAmount,case when cbd.BrokerageDetail_BrkgFor=1 then 'All'  else (select Products_Name from Master_Products where Products_ID=cbd.BrokerageDetail_ProductID) end as BrokerageDetail_BrkgFor,case when cbd.BrokerageDetail_BrkgType=1 then 'Delivery' when cbd.BrokerageDetail_BrkgType=2 then 'Square-Off' when cbd.BrokerageDetail_BrkgType=3 then 'Exercise' when cbd.BrokerageDetail_BrkgType=4 then 'Assignment' when cbd.BrokerageDetail_BrkgType=6 then 'Delivery CloseValue' else 'Final Settlement' end as BrokerageDetail_BrkgType,case when  cbd.BrokerageDetail_TranType=1 then 'Purchase' when cbd.BrokerageDetail_TranType=2 then 'Sale' when cbd.BrokerageDetail_TranType=3 then 'Both' when cbd.BrokerageDetail_TranType=4 then 'FirstLeg' when cbd.BrokerageDetail_TranType=5 then 'SecondLeg' when cbd.BrokerageDetail_TranType=6 then 'HigherLeg' when cbd.BrokerageDetail_TranType=7  then 'LowerLeg' when cbd.BrokerageDetail_TranType=8 then 'Daily' when cbd.BrokerageDetail_TranType=9 then 'DailySecond' when cbd.BrokerageDetail_TranType=10 then 'Carry' else 'CarrySecond' end as BrokerageDetail_TranType,case when cbd.BrokerageDetail_InstrType=1 then 'All' when cbd.BrokerageDetail_InstrType=2 then 'Equity' when cbd.BrokerageDetail_InstrType=3 then 'Bonds'  when cbd.BrokerageDetail_InstrType=4 then 'Debt' when cbd.BrokerageDetail_InstrType=5 then 'ETFs' when cbd.BrokerageDetail_InstrType=6 then ' Equity Futures' when cbd.BrokerageDetail_InstrType=7 then 'Equity Options'  when cbd.BrokerageDetail_InstrType=8 then 'Index Futures'when cbd.BrokerageDetail_InstrType=9 then 'Index Options'when cbd.BrokerageDetail_InstrType=10 then 'All Futures'when cbd.BrokerageDetail_InstrType=11 then 'All Options' when cbd.BrokerageDetail_InstrType=12 then 'Comm Futures'when cbd.BrokerageDetail_InstrType=13 then 'Comm Options'  when cbd.BrokerageDetail_InstrType=14 then 'All Futures' else 'All Options' end as BrokerageDetail_InstrType ", "cbd.BrokerageDetail_MainID='" + dtBRMain.Rows[0]["BrokerageMain_ID"].ToString() + "' and cbd.BrokerageDetail_MainID=cbm.BrokerageMain_ID");
                        }

                        if (dtBRMain.Rows.Count > 0 && dtBRDetail.Rows.Count > 0 && dtCharge.Rows.Count > 0)
                        {

                            dispTbl = dispTbl + "<tr  ><td  colspan=\"2\" style=\"padding:5px 6px 5px 6px\" align=\"center\"><b>Brokerage Setup in " + dtsegName.Rows[0]["SegmentName"].ToString();
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

                            dispTbl = dispTbl + "<tr  ><td align=\"left\"  valign=\"top\"  style=\"padding:5px 6px 5px 6px\">";

                            dispTbl = dispTbl + "<table      cellpadding=\"1\" cellspacing=\"1\"  style=\" font-family: verdana; font-size: 10px;\"><tr >";
                            dispTbl = dispTbl + "<td valign=\"top\">";

                            dispTbl = dispTbl + "<table  border=\"1\"     cellpadding=\"1\" cellspacing=\"1\"  style=\" font-family: verdana; font-size: 10px;border: 1px solid black;\">";
                            dispTbl = dispTbl + "<tr ><td align=\"left\"  valign=\"top\" colspan=\"8\"><b>Brokerage[General] &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<b>Company:" + dtBRMain.Rows[0]["BrokerageMain_CompanyID"].ToString().Trim() + "&nbsp;&nbsp;&nbsp;&nbsp;<b>Date:" + dtBRMain.Rows[0]["BrokerageMain_FromDate"].ToString().Trim() + "</td></tr>";
                            dispTbl = dispTbl + "<tr  ><td align=\"left\"  valign=\"top\">Brkg Decimals:</td><td align=\"left\"  valign=\"top\">" + dtBRMain.Rows[0]["BrokerageMain_BrkgDecimals"].ToString().Trim() + "</td><td align=\"left\"  valign=\"top\">Brkg.Rd-Off:</td><td align=\"left\"  valign=\"top\">" + dtBRMain.Rows[0]["BrokerageMain_BrkgRoundPattern"].ToString().Trim() + "</td><td align=\"left\"  valign=\"top\">Min Daily Brkg:</td><td align=\"left\"  valign=\"top\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dtBRMain.Rows[0]["BrokerageMain_MinDailyBrkg"].ToString().Trim())) + "</td><td align=\"left\"  valign=\"top\">Min Sqr Brkg/Shr:</td><td align=\"left\"  valign=\"top\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dtBRMain.Rows[0]["BrokerageMain_MinSqrPerShare"].ToString().Trim())) + "</td></tr>";
                            dispTbl = dispTbl + "<tr  ><td align=\"left\"  valign=\"top\">Net Amt Decimal:</td><td align=\"left\"  valign=\"top\">" + dtBRMain.Rows[0]["BrokerageMain_NetDecimals"].ToString().Trim() + "</td><td align=\"left\"  valign=\"top\">Mkt. Rd-Off:</td><td align=\"left\"  valign=\"top\">" + dtBRMain.Rows[0]["BrokerageMain_MktRoundPattern"].ToString().Trim() + "</td><td align=\"left\"  valign=\"top\">Max Daily Brkg:</td><td align=\"left\"  valign=\"top\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dtBRMain.Rows[0]["BrokerageMain_MaxDailyBrkg"].ToString().Trim())) + "</td><td align=\"left\"  valign=\"top\">Max Sqr Brkg/Shr:</td><td align=\"left\"  valign=\"top\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dtBRMain.Rows[0]["BrokerageMain_MaxSqrPerShare"].ToString().Trim())) + "</td></tr>";
                            dispTbl = dispTbl + "<tr  ><td align=\"left\"  valign=\"top\">Mkt Rate Decimal:</td><td align=\"left\"  valign=\"top\">" + dtBRMain.Rows[0]["BrokerageMain_MktDecimals"].ToString().Trim() + "</td><td align=\"left\"  valign=\"top\">Net Rd-Off:</td><td align=\"left\"  valign=\"top\">" + dtBRMain.Rows[0]["BrokerageMain_NetRoundPattern"].ToString().Trim() + "</td><td align=\"left\"  valign=\"top\">Min Del Brkg/Shr:</td><td align=\"left\"  valign=\"top\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dtBRMain.Rows[0]["BrokerageMain_MinDelPerShare"].ToString().Trim())) + "</td><td align=\"left\"  valign=\"top\">Min Brkg/Order:</td><td align=\"left\"  valign=\"top\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dtBRMain.Rows[0]["BrokerageMain_MinBrkgPerOrder"].ToString().Trim())) + "</td></tr>";
                            dispTbl = dispTbl + "<tr  ><td align=\"left\"  valign=\"top\">Trd Avg.Type:</td><td align=\"left\"  valign=\"top\">" + dtBRMain.Rows[0]["BrokerageMain_AverageType"].ToString().Trim() + "</td><td align=\"left\"  valign=\"top\">Cont. Pattern:</td><td align=\"left\"  valign=\"top\">" + dtBRMain.Rows[0]["BrokerageMain_ContractPattern"].ToString().Trim() + "</td><td align=\"left\"  valign=\"top\">Max Del Brkg/Shr:</td><td align=\"left\"  valign=\"top\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dtBRMain.Rows[0]["BrokerageMain_MaxDelPerShare"].ToString().Trim())) + "</td><td align=\"left\"  valign=\"top\">Max Brkg/Order:</td><td align=\"left\"  valign=\"top\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dtBRMain.Rows[0]["BrokerageMain_MaxBrkgPerOrder"].ToString().Trim())) + "</td></tr>";
                            dispTbl = dispTbl + "</table>";

                            dispTbl = dispTbl + "</td>";
                            dispTbl = dispTbl + "<td style=\"padding:0px 6px 0px 6px\">";
                            if (dtCharge.Rows.Count > 0)
                            {
                                // dispTbl = dispTbl + "<tr   align=\"left\"><td align=\"left\" colspan=\"2\"  valign=\"top\"  style=\"padding:5px 6px 5px 6px\">";
                                dispTbl = dispTbl + "<table  border=\"1\"     cellpadding=\"1\" cellspacing=\"1\" style=\" font-family: verdana; font-size: 10px;border: 1px solid black;\"><tr align=\"left\" ><td align=\"left\"  valign=\"top\" colspan=\"2\"><b>Charge Setup[Details]</td></tr><tr  ><td>&nbsp;Charges</td><td>&nbsp;Basis</td></tr>";
                                for (int p = 0; p <= dtCharge.Rows.Count - 1; p++)
                                {
                                    dispTbl = dispTbl + "<tr  ><td align=\"left\">" + dtCharge.Rows[p]["ChargeSetup_ChargeType"].ToString() + "</td><td align=\"left\">" + dtCharge.Rows[p]["ChargeSetup_ChargeBasis"].ToString() + "</td></tr>";
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
                            dispTbl = dispTbl + "<tr   align=\"left\"><td align=\"left\" colspan=\"2\"  valign=\"top\"  style=\"padding:5px 6px 5px 6px\">";
                            dispTbl = dispTbl + "<table  border=\"1\"     cellpadding=\"1\" cellspacing=\"1\" style=\" font-family: verdana; font-size: 10px;border: 1px solid black;\"><tr align=\"left\" ><td align=\"left\"  valign=\"top\" colspan=\"9\"><b>Brokerage Setup[Details]:</td></tr><tr  ><td>Mrkt. Segment</td><td>Brkg. Type</td><td>Brkg. For</td><td>Tran. Type</td><td>Inst. Type</td><td>Flat Amt.</td><td>Rate</td><td>Min. Amt.</td><td>Brkg. Slab</td></tr>";
                            for (int n = 0; n <= dtBRDetail.Rows.Count - 1; n++)
                            {
                                dispTbl = dispTbl + "<tr  ><td align=\"left\">&nbsp;" + dtBRDetail.Rows[n]["BrokerageDetail_MktSegment"].ToString() + "</td><td align=\"left\">&nbsp;" + dtBRDetail.Rows[n]["BrokerageDetail_BrkgType"].ToString() + "</td><td align=\"left\">&nbsp;" + dtBRDetail.Rows[n]["BrokerageDetail_BrkgFor"].ToString() + "</td><td align=\"left\">" + dtBRDetail.Rows[n]["BrokerageDetail_TranType"].ToString() + "</td><td align=\"left\">" + dtBRDetail.Rows[n]["BrokerageDetail_InstrType"].ToString() + "</td><td align=\"left\">&nbsp;" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dtBRDetail.Rows[n]["BrokerageDetail_FlatRate"].ToString())) + "</td><td align=\"left\">&nbsp;" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dtBRDetail.Rows[n]["BrokerageDetail_Rate"].ToString())) + "</td><td align=\"left\">&nbsp;" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(Convert.ToDecimal(dtBRDetail.Rows[n]["BrokerageDetail_MinAmount"].ToString())) + "</td><td align=\"left\">&nbsp;" + dtBRDetail.Rows[n]["BrokerageDetail_SlabCode"].ToString() + "</td></tr>";
                            }
                            dispTbl = dispTbl + "</table>";
                            dispTbl = dispTbl + "</td></tr>";

                        }

                        dtCH.Clear();
                        dtCharge.Clear();
                        dtBRMain.Clear();
                        dtBRDetail.Clear();
                        dtCHGRUP.Clear();


                    }


                }



                //-----------Brokerage  --------

                dispTbl = dispTbl + "</table>";
                MainContainer.InnerHtml = dispTbl.ToString();
                //Session["MainDataTable"] = DtMain;
                //HdPageNo.Value = PageNo.ToString();
                //if (DtMain.Rows.Count == 1)
                //{
                //    trButton.Visible = false;
                //}
                //else
                //{

                //}


            }

        }
    }
}