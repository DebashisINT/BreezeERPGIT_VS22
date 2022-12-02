using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using CrystalDecisions.CrystalReports.Engine;
namespace ERP.OMS.Reports
{
    public partial class Reports_frm_Report_ContactDetails : System.Web.UI.Page, System.Web.UI.ICallbackEventHandler
    {
        string data;
        static string SubId;
        static string BranchId;
        static string SegmentID;
        string[,] Customerid;
        DataTable DtMain = new DataTable();
        static DataSet ds = new DataSet();
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        BusinessLogicLayer.Converter ObjConvert = new BusinessLogicLayer.Converter();
        int PageNo = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtsegselected.Attributes.Add("onkeyup", "showOptionsforSunAc(this,'selectSubAccountForMainAccountAndBranch',event," + Session["userlastsegment"].ToString() + ")");

                rdAll.Attributes.Add("OnClick", "MainAll('Client','all')");
                rdSelected.Attributes.Add("OnClick", "MainAll('Client','Selc')");
                rdAllBranch.Attributes.Add("OnClick", "MainAll('Branch','all')");
                rdSelBrnh.Attributes.Add("OnClick", "MainAll('Branch','Selc')");
                trButton.Visible = false;
            }
            String cbReference = Page.ClientScript.GetCallbackEventReference(this, "arg", "ReceiveServerData", "context");
            String callbackScript = "function CallServer(arg, context){ " + cbReference + ";}";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "CallServer", callbackScript, true);
            Page.ClientScript.RegisterStartupScript(GetType(), "callHight", "<script language='Javascript'>height();</script>");
            //trButton.Visible = false;
        }

        string ICallbackEventHandler.GetCallbackResult()
        {
            return data;
        }
        void ICallbackEventHandler.RaiseCallbackEvent(string eventArgument)
        {
            string id = eventArgument.ToString();
            string[] idlist = id.Split('~');
            string[] cl = idlist[1].Split(',');
            string str = "";
            string str1 = "";
            for (int i = 0; i < cl.Length; i++)
            {
                string[] val = cl[i].Split(';');
                if (str == "")
                {
                    str = "'" + val[0] + "'";
                    str1 = val[0] + ";" + val[1];
                }
                else
                {
                    str += ",'" + val[0] + "'";
                    str1 += "," + val[0] + ";" + val[1];
                }
            }
            if (idlist[0] == "Client")
            {
                SubId = str;
                Session["KeyValSegment"] = str;
                data = "Client~" + str1;
            }
            if (idlist[0] == "Branch")
            {
                BranchId = str;
                Session["KeyVal"] = str;
                data = "Branch~" + str1;
            }
        }

        protected void dpHitDataBase_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {


        }
        protected void Button1_Click(object sender, EventArgs e)
        {

            HdPageNo.Value = "0";
            CreateDataTable();
            fillHTML();
            if (DtMain.Rows.Count > 1)
            {
                trButton.Visible = true;
            }
            else if (DtMain.Rows.Count == 0)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "Message", "<script language='JavaScript'>alert('No Record To Display');</script>");
            }
        }
        public void fillHTML()
        {
            String SEGMENTMORE1 = "";
            String SEGMENTMORE2 = "";
            int flag = 1;

            DataTable dtSegment = new DataTable();
            DataTable dtBrokerageDtl = new DataTable();

            if (rdAll.Checked == true)
            {

                dtSegment = oDBEngine.GetDataTable("tbl_master_contactexchange", "crg_exchange,crg_cntid", null, "crg_cntid");
                string expression1 = " Auto_Id='" + PageNo.ToString() + "'";
                DataRow[] target1 = DtMain.Select(expression1);
                Session["KeyValSegment"] = "'" + target1[0]["internalid"].ToString() + "'";
                dtBrokerageDtl = oDBEngine.GetDataTable("(select BrokerageDetail_MainID,brokeragemain_mindelpershare,brokeragemain_mindailybrkg,brokeragemain_minbrkgperorder,brokeragemain_customerid,cbd.BrokerageDetail_ProductID,case when cbd.BrokerageDetail_MktSegment=1 then 'All' when  cbd.BrokerageDetail_MktSegment=2 then 'Rolling' when  cbd.BrokerageDetail_MktSegment=3 then 'T2T' when  cbd.BrokerageDetail_MktSegment=4 then 'Physical'  when  cbd.BrokerageDetail_MktSegment=5 then 'Institutional' when cbd.BrokerageDetail_MktSegment=6 then 'Auction' else ' ' end as BrokerageDetail_MktSegment,cbd.BrokerageDetail_SlabCode,cbd.BrokerageDetail_ID,cast(cbd.BrokerageDetail_FlatRate  as decimal(18,2))as BrokerageDetail_FlatRate,cbd.BrokerageDetail_Rate,cast(cbd.BrokerageDetail_MinAmount  as decimal(18,2)) as BrokerageDetail_MinAmount,case when cbd.BrokerageDetail_BrkgFor=1 then 'All'  else (select Products_Name from Master_Products where Products_ID=cbd.BrokerageDetail_ProductID) end as BrokerageDetail_BrkgFor,case when cbd.BrokerageDetail_BrkgType=1 then 'Delivery' when cbd.BrokerageDetail_BrkgType=2 then 'Square-Off' when cbd.BrokerageDetail_BrkgType=3 then 'Exercise' when cbd.BrokerageDetail_BrkgType=4 then 'Assignment' else 'Final Settlement' end as BrokerageDetail_BrkgType,case when  cbd.BrokerageDetail_TranType=1 then 'Purchase' when cbd.BrokerageDetail_TranType=2 then 'Sale' when cbd.BrokerageDetail_TranType=3 then 'Both' when cbd.BrokerageDetail_TranType=4 then 'FirstLeg' when cbd.BrokerageDetail_TranType=5 then 'SecondLeg' when cbd.BrokerageDetail_TranType=6 then 'HigherLeg' when cbd.BrokerageDetail_TranType=7  then 'LowerLeg' when cbd.BrokerageDetail_TranType=8 then 'Daily' when cbd.BrokerageDetail_TranType=9 then 'DailySecond' when cbd.BrokerageDetail_TranType=10 then 'Carry' else 'CarrySecond' end as BrokerageDetail_TranType,case when cbd.BrokerageDetail_InstrType=1 then 'All' when cbd.BrokerageDetail_InstrType=2 then 'Equity' when cbd.BrokerageDetail_InstrType=3 then 'Bonds'  when cbd.BrokerageDetail_InstrType=4 then 'Debt' when cbd.BrokerageDetail_InstrType=5 then 'ETFs' when cbd.BrokerageDetail_InstrType=6 then ' Equity Futures' when cbd.BrokerageDetail_InstrType=7 then 'Equity Options'  when cbd.BrokerageDetail_InstrType=8 then 'Index Futures'when cbd.BrokerageDetail_InstrType=9 then 'Index Options'when cbd.BrokerageDetail_InstrType=10 then 'All Futures'when cbd.BrokerageDetail_InstrType=11 then 'All Options' when cbd.BrokerageDetail_InstrType=12 then 'Comm Futures'when cbd.BrokerageDetail_InstrType=13 then 'Comm Options'  when cbd.BrokerageDetail_InstrType=14 then 'All Futures' else 'All Options' end as BrokerageDetail_InstrType from Config_BrokerageDetail as cbd ,Config_BrokerageMain as cbm where brokeragemain_customerid in(" + Session["KeyValSegment"].ToString() + ") and  cbd.BrokerageDetail_MainID=cbm.BrokerageMain_ID)as a left outer join config_chargesetup on a.BrokerageDetail_MainID=chargesetup_mainid and a.brokeragemain_customerid in(" + Session["KeyValSegment"].ToString() + ")", "a.*,ChargeSetup_ChargeType,ChargeSetup_ChargeBasis,ChargeSetup_ChargeGroup", null);
            }
            else if (rdSelected.Checked == true)
            {

                DtMain = oDBEngine.GetDataTable("(select isnull(a.add_address1,b.add_address1)as address,isnull(a.city,b.city) as city,isnull(a.add_pin,b.add_pin)as pin,isnull(a.state,b.state) as state,isnull(a.add_cntid,b.add_cntid) as internalid from(select top 1 add_address1,add_cntid,(select city_name from tbl_master_city where city_id=tbl_master_address.add_city)as city,add_pin,(select state from tbl_master_state where id=tbl_master_address.add_state)as state from tbl_master_address where add_cntid in(" + Session["KeyValSegment"].ToString() + ") )as a full outer join (select top 1 add_address1,add_cntid,(select city_name from tbl_master_city where city_id=tbl_master_address.add_city)as city,add_pin,(select state from tbl_master_state where id=tbl_master_address.add_state)as state from tbl_master_address where add_cntid in(" + Session["KeyValSegment"].ToString() + ")  )as b on a.add_cntid=b.add_cntid) as tbl_address right outer join tbl_master_contact on tbl_address.internalid=tbl_master_contact.cnt_internalid left outer join (select isnull(c.landno,d.landno)as LandOrMobile,isnull(c.phf_cntid,d.phf_cntid)as phf_cntid from(select phf_phonenumber,phf_type,phf_cntid,isnull(phf_countrycode,'')+isnull(phf_areacode,'')+isnull(phf_phonenumber,'') as landno from tbl_master_phonefax where phf_cntid in(" + Session["KeyValSegment"].ToString() + ") and phf_type='Mobile')as c full outer join (select phf_phonenumber,phf_type,phf_cntid,isnull(phf_countrycode,'')+isnull(phf_areacode,'')+isnull(phf_phonenumber,'') as landno from tbl_master_phonefax where phf_cntid in(" + Session["KeyValSegment"].ToString() + ") and phf_type='Office')as d on c.phf_cntid=d.phf_cntid) as tbl_phone on tbl_phone.phf_cntid=tbl_master_contact.cnt_internalid left outer join (select cbd_accountnumber,bnk_bankname,cbd_cntid from tbl_master_bank left outer join tbl_trans_contactbankdetails on bnk_id=cbd_bankcode where cbd_accountcategory='Default' and cbd_cntid in(" + Session["KeyValSegment"].ToString() + "))as tbl_bank on tbl_bank.cbd_cntid=tbl_master_contact.cnt_internalid left outer join ( select e.dpd_clientid,e.dpd_dpcode,f.dp_dpname,e.dpd_cntid from tbl_master_contactdpdetails as e inner join tbl_master_depositoryParticipants as f on substring(e.dpd_dpcode,1,8)=substring(f.dp_dpid,1,8) and dpd_accounttype='Default') as tbl_dp on tbl_dp.dpd_cntid=tbl_master_contact.cnt_internalid", "cnt_ucc,isnull(cnt_firstname,'')+' '+isnull(cnt_middlename,'')+' '+isnull(cnt_lastname,'')as Name,cnt_clienttype,tbl_address.*,tbl_phone.*,isnull((select eml_email from tbl_master_email where tbl_master_email.eml_type='Official' and eml_cntId=tbl_master_contact.cnt_internalid),'') as email,isnull((select gpm_description from tbl_master_groupmaster where gpm_id in(select top 1 grp_groupmaster  from tbl_trans_group where grp_grouptype='Family' and grp_contactid=tbl_master_contact.cnt_internalid)),'') as Group1,isnull((select crg_number  from tbl_master_contactregistration where crg_type='Pancard' and crg_cntid=tbl_master_contact.cnt_internalid),'')as pan,tbl_bank.cbd_accountnumber,tbl_bank.bnk_bankname,tbl_dp.dpd_clientid,tbl_dp.dpd_dpcode,tbl_dp.dp_dpname,(select lgl_legalstatus from tbl_master_legalstatus where lgl_id=tbl_master_contact.cnt_legalStatus)as status", "tbl_master_contact.cnt_internalid in(" + Session["KeyValSegment"].ToString() + ")", "tbl_master_contact.cnt_internalid");
                dtSegment = oDBEngine.GetDataTable("tbl_master_contactexchange", "crg_exchange,crg_cntid", "crg_cntid in(" + Session["KeyValSegment"].ToString() + ")", "crg_cntid");
                //dtBrokerageDtl = oDBEngine.GetDataTable("(select BrokerageDetail_MainID,brokeragemain_mindelpershare,brokeragemain_mindailybrkg,brokeragemain_minbrkgperorder,brokeragemain_customerid,cbd.BrokerageDetail_ProductID,case when cbd.BrokerageDetail_MktSegment=1 then 'All' when  cbd.BrokerageDetail_MktSegment=2 then 'Rolling' when  cbd.BrokerageDetail_MktSegment=3 then 'T2T' when  cbd.BrokerageDetail_MktSegment=4 then 'Physical'  when  cbd.BrokerageDetail_MktSegment=5 then 'Institutional' when cbd.BrokerageDetail_MktSegment=6 then 'Auction' else ' ' end as BrokerageDetail_MktSegment,cbd.BrokerageDetail_SlabCode,cbd.BrokerageDetail_ID,cast(cbd.BrokerageDetail_FlatRate  as decimal(18,2))as BrokerageDetail_FlatRate,cbd.BrokerageDetail_Rate,cast(cbd.BrokerageDetail_MinAmount  as decimal(18,2)) as BrokerageDetail_MinAmount,case when cbd.BrokerageDetail_BrkgFor=1 then 'All'  else (select Products_Name from Master_Products where Products_ID=cbd.BrokerageDetail_ProductID) end as BrokerageDetail_BrkgFor,case when cbd.BrokerageDetail_BrkgType=1 then 'Delivery' when cbd.BrokerageDetail_BrkgType=2 then 'Square-Off' when cbd.BrokerageDetail_BrkgType=3 then 'Exercise' when cbd.BrokerageDetail_BrkgType=4 then 'Assignment' else 'Final Settlement' end as BrokerageDetail_BrkgType,case when  cbd.BrokerageDetail_TranType=1 then 'Purchase' when cbd.BrokerageDetail_TranType=2 then 'Sale' when cbd.BrokerageDetail_TranType=3 then 'Both' when cbd.BrokerageDetail_TranType=4 then 'FirstLeg' when cbd.BrokerageDetail_TranType=5 then 'SecondLeg' when cbd.BrokerageDetail_TranType=6 then 'HigherLeg' when cbd.BrokerageDetail_TranType=7  then 'LowerLeg' when cbd.BrokerageDetail_TranType=8 then 'Daily' when cbd.BrokerageDetail_TranType=9 then 'DailySecond' when cbd.BrokerageDetail_TranType=10 then 'Carry' else 'CarrySecond' end as BrokerageDetail_TranType,case when cbd.BrokerageDetail_InstrType=1 then 'All' when cbd.BrokerageDetail_InstrType=2 then 'Equity' when cbd.BrokerageDetail_InstrType=3 then 'Bonds'  when cbd.BrokerageDetail_InstrType=4 then 'Debt' when cbd.BrokerageDetail_InstrType=5 then 'ETFs' when cbd.BrokerageDetail_InstrType=6 then ' Equity Futures' when cbd.BrokerageDetail_InstrType=7 then 'Equity Options'  when cbd.BrokerageDetail_InstrType=8 then 'Index Futures'when cbd.BrokerageDetail_InstrType=9 then 'Index Options'when cbd.BrokerageDetail_InstrType=10 then 'All Futures'when cbd.BrokerageDetail_InstrType=11 then 'All Options' when cbd.BrokerageDetail_InstrType=12 then 'Comm Futures'when cbd.BrokerageDetail_InstrType=13 then 'Comm Options'  when cbd.BrokerageDetail_InstrType=14 then 'All Futures' else 'All Options' end as BrokerageDetail_InstrType from Config_BrokerageDetail as cbd ,Config_BrokerageMain as cbm where brokeragemain_customerid in(" + Session["KeyValSegment"].ToString() + ") and  cbd.BrokerageDetail_MainID=cbm.BrokerageMain_ID)as a left outer join config_chargesetup on a.BrokerageDetail_MainID=chargesetup_mainid and a.brokeragemain_customerid in(" + Session["KeyValSegment"].ToString() + ")", "a.*,ChargeSetup_ChargeType,case ChargeSetup_ChargeBasis when '1' then 'Inclusive' when '2' then 'Exclusive' else 'Not Applicable' end as ChargeSetup_ChargeBasis,ChargeSetup_ChargeGroup", null);
                Customerid = oDBEngine.GetFieldValue("trans_chargegroupmembers", "chargegroupmembers_groupcode", "chargegroupmembers_customerid in(" + Session["KeyValSegment"].ToString() + ") and (chargegroupmembers_untildate>=getdate() or chargegroupmembers_untildate is null)", 1, "chargegroupmembers_untildate desc");
                Session["KeyValSegment1"] = "'" + Customerid[0, 0].ToString().Trim() + "'";
                if (Customerid.Length > 1)
                {
                    for (int i = 1; i < int.Parse(Customerid.Length.ToString()); i++)
                    {
                        Session["KeyValSegment1"] = Session["KeyValSegment"].ToString() + "," + "'" + Customerid[i, 0].ToString().Trim() + "'";
                    }
                }
                dtBrokerageDtl = oDBEngine.GetDataTable("(Select a.*,ChargeSetup_ChargeType,case ChargeSetup_ChargeBasis when '1' then 'Inclusive' when '2' then 'Exclusive' else 'Not Applicable' end as ChargeSetup_ChargeBasis,ChargeSetup_ChargeGroup from (select BrokerageDetail_MainID,brokeragemain_mindelpershare,BrokerageMain_SEGMENTID,brokeragemain_mindailybrkg,brokeragemain_minbrkgperorder,brokeragemain_customerid,cbd.BrokerageDetail_ProductID,case when cbd.BrokerageDetail_MktSegment=1 then 'All' when  cbd.BrokerageDetail_MktSegment=2 then 'Rolling' when  cbd.BrokerageDetail_MktSegment=3 then 'T2T' when  cbd.BrokerageDetail_MktSegment=4 then 'Physical'  when  cbd.BrokerageDetail_MktSegment=5 then 'Institutional' when cbd.BrokerageDetail_MktSegment=6 then 'Auction' else ' ' end as BrokerageDetail_MktSegment,cbd.BrokerageDetail_SlabCode,cbd.BrokerageDetail_ID,cast(cbd.BrokerageDetail_FlatRate  as decimal(18,2))as BrokerageDetail_FlatRate,cbd.BrokerageDetail_Rate,cast(cbd.BrokerageDetail_MinAmount  as decimal(18,2)) as BrokerageDetail_MinAmount,case when cbd.BrokerageDetail_BrkgFor=1 then 'All'  else (select Products_Name from Master_Products where Products_ID=cbd.BrokerageDetail_ProductID) end as BrokerageDetail_BrkgFor,case when cbd.BrokerageDetail_BrkgType=1 then 'Delivery' when cbd.BrokerageDetail_BrkgType=2 then 'Square-Off' when cbd.BrokerageDetail_BrkgType=3 then 'Exercise' when cbd.BrokerageDetail_BrkgType=4 then 'Assignment' else 'Final Settlement' end as BrokerageDetail_BrkgType,case when  cbd.BrokerageDetail_TranType=1 then 'Purchase' when cbd.BrokerageDetail_TranType=2 then 'Sale' when cbd.BrokerageDetail_TranType=3 then 'Both' when cbd.BrokerageDetail_TranType=4 then 'FirstLeg' when cbd.BrokerageDetail_TranType=5 then 'SecondLeg' when cbd.BrokerageDetail_TranType=6 then 'HigherLeg' when cbd.BrokerageDetail_TranType=7  then 'LowerLeg' when cbd.BrokerageDetail_TranType=8 then 'Daily' when cbd.BrokerageDetail_TranType=9 then 'DailySecond' when cbd.BrokerageDetail_TranType=10 then 'Carry' else 'CarrySecond' end as BrokerageDetail_TranType,case when cbd.BrokerageDetail_InstrType=1 then 'All' when cbd.BrokerageDetail_InstrType=2 then 'Equity' when cbd.BrokerageDetail_InstrType=3 then 'Bonds'  when cbd.BrokerageDetail_InstrType=4 then 'Debt' when cbd.BrokerageDetail_InstrType=5 then 'ETFs' when cbd.BrokerageDetail_InstrType=6 then ' Equity Futures' when cbd.BrokerageDetail_InstrType=7 then 'Equity Options'  when cbd.BrokerageDetail_InstrType=8 then 'Index Futures'when cbd.BrokerageDetail_InstrType=9 then 'Index Options'when cbd.BrokerageDetail_InstrType=10 then 'All Futures'when cbd.BrokerageDetail_InstrType=11 then 'All Options' when cbd.BrokerageDetail_InstrType=12 then 'Comm Futures'when cbd.BrokerageDetail_InstrType=13 then 'Comm Options'  when cbd.BrokerageDetail_InstrType=14 then 'All Futures' else 'All Options' end as BrokerageDetail_InstrType from Config_BrokerageDetail as cbd ,Config_BrokerageMain as cbm where brokeragemain_customerid in(" + Session["KeyValSegment1"].ToString() + ") and  cbd.BrokerageDetail_MainID=cbm.BrokerageMain_ID)as a left outer join config_chargesetup on a.BrokerageDetail_MainID=chargesetup_mainid and a.brokeragemain_customerid in(" + Session["KeyValSegment1"].ToString() + "))as k inner join TBL_MASTER_COMPANYEXCHANGE as e on e.exch_internalid=k.brokeragemain_segmentid", "k.*,case when e.exch_segmentid='CM' then 'NSE-CM' when e.exch_segmentid='FO' then 'NSE-FO' else e.exch_segmentid end as seg", null);


            }

            string Format = "<table cellspacing=\"1\" cellpadding=\"1\"  style=\"border:slid 1px blue;background:white;width:100%\">";
            Format = "<script language=\"javascript\" type=\"text/javascript\">";
            Format += "function height(){ if(document.body.scrollHeight>=500)window.frameElement.height = document.body.scrollHeight;else window.frameElement.height = '1050px'; window.frameElement.Width = document.body.scrollWidth;}";
            Format += "</script>";
            Format += "<tr style=\"background-color: " + GetRowColor(flag) + "\"><td style=\"font-weight:bold; color: Maroon\" align=\"left\" >Particulars</td><td  style=\"font-weight:bold; color: Maroon\" align=\"left\">Client Details</td></tr>";
            flag = flag + 1;
            Format += "<tr style=\"background-color: " + GetRowColor(flag) + "\"><td style=\"font-weight:bold\">" + DtMain.Rows[PageNo][1].ToString() + "[" + DtMain.Rows[PageNo][0].ToString() + "]</td><td></td></tr>";
            flag = flag + 1;
            Format += "<tr style=\"background-color: " + GetRowColor(flag) + "\"><td > UCC(Alias):</td><td>" + DtMain.Rows[PageNo][0].ToString() + "</td></tr>";
            flag = flag + 1;
            Format += "<tr style=\"background-color: " + GetRowColor(flag) + "\"><td > Client Type:</td><td>" + DtMain.Rows[PageNo][2].ToString() + "</td></tr>";
            flag = flag + 1;
            Format += "<tr style=\"background-color: " + GetRowColor(flag) + "\"><td > Client Name:</td><td>" + DtMain.Rows[PageNo][1].ToString() + "</td></tr>";
            flag = flag + 1;
            Format += "<tr style=\"background-color: " + GetRowColor(flag) + "\"><td > Address:</td><td>" + DtMain.Rows[PageNo][3].ToString() + "</td></tr>";
            flag = flag + 1;
            Format += "<tr style=\"background-color: " + GetRowColor(flag) + "\"><td > City:</td><td>" + DtMain.Rows[PageNo][4].ToString() + "</td></tr>";
            flag = flag + 1;
            Format += "<tr style=\"background-color: " + GetRowColor(flag) + "\"><td > Pin:</td><td>" + DtMain.Rows[PageNo][5].ToString() + "</td></tr>";
            flag = flag + 1;
            Format += "<tr style=\"background-color: " + GetRowColor(flag) + "\"><td > State:</td><td>" + DtMain.Rows[PageNo][6].ToString() + "</td></tr>";
            flag = flag + 1;
            Format += "<tr style=\"background-color: " + GetRowColor(flag) + "\"><td > Phone:</td><td>" + DtMain.Rows[PageNo][8].ToString() + "</td></tr>";
            flag = flag + 1;
            Format += "<tr style=\"background-color: " + GetRowColor(flag) + "\"><td > Fax:</td><td></td></tr>";
            flag = flag + 1;
            Format += "<tr style=\"background-color: " + GetRowColor(flag) + "\"><td > Email:</td><td>" + DtMain.Rows[PageNo][10].ToString() + "</td></tr>";
            flag = flag + 1;
            Format += "<tr style=\"background-color: " + GetRowColor(flag) + "\"><td > Family Group:</td><td>" + DtMain.Rows[PageNo][11].ToString() + "</td></tr>";
            flag = flag + 1;
            Format += "<tr style=\"background-color: " + GetRowColor(flag) + "\"><td > Pan No:</td><td>" + DtMain.Rows[PageNo][12].ToString() + "</td></tr>";
            flag = flag + 1;
            Format += "<tr style=\"background-color: " + GetRowColor(flag) + "\"><td > Registration Received:</td><td></td></tr>";
            flag = flag + 1;
            Format += "<tr style=\"background-color: " + GetRowColor(flag) + "\"><td > Client Category:</td><td>" + DtMain.Rows[PageNo]["status"].ToString() + "</td></tr>";
            //flag = flag + 1;
            string expression = " crg_cntid='" + DtMain.Rows[PageNo][7].ToString() + "'";
            DataRow[] target = dtSegment.Select(expression);
            if (target.Length > 0)
            {
                if (target.Length == 1)
                {

                    flag = flag + 1;
                    Format += "<tr style=\"background-color: " + GetRowColor(flag) + "\"><td > Registered Segment</td><td>" + target[0][0].ToString() + "</td></tr>";
                    SEGMENTMORE1 = target[0][0].ToString();
                    if (SEGMENTMORE1 == "NSE - CM")
                    {
                        SEGMENTMORE1 = "NSE-CM";
                    }
                    else
                    {
                        if (SEGMENTMORE1 == "NSE - FO")
                        {
                            SEGMENTMORE1 = "NSE-FO";
                        }
                    }
                }
                else if (target.Length > 1)
                {
                    flag = flag + 1;
                    Format += "<tr style=\"background-color: " + GetRowColor(flag) + "\"><td > Registered Segment</td><td>" + target[0][0].ToString() + ";" + target[1][0].ToString() + "</td></tr>";
                    SEGMENTMORE1 = target[0][0].ToString();
                    SEGMENTMORE2 = target[1][0].ToString();
                    if (SEGMENTMORE1 == "NSE - CM")
                    {
                        SEGMENTMORE1 = "NSE-CM";
                    }
                    else
                    {
                        if (SEGMENTMORE1 == "NSE - FO")
                        {
                            SEGMENTMORE1 = "NSE-FO";
                        }
                    }
                    if (SEGMENTMORE2 == "NSE - CM")
                    {
                        SEGMENTMORE2 = "NSE-CM";
                    }
                    else
                    {
                        if (SEGMENTMORE2 == "NSE - FO")
                        {
                            SEGMENTMORE2 = "NSE-FO";
                        }
                    }
                }

            }

            expression = "brokeragemain_customerid='" + DtMain.Rows[PageNo][7].ToString() + "' ";
            flag = flag + 1;
            Format += "<tr style=\"background-color: " + GetRowColor(flag) + "\"><td>Dp Details:</td><td colspan=\"2\"><table><tr><td style=\"font-weight:bold\">Primary Client Id:</td><td>" + DtMain.Rows[PageNo]["dpd_clientid"].ToString() + "</td><td style=\"font-weight:bold\"> DPID: </td><td> " + DtMain.Rows[PageNo]["dpd_dpcode"].ToString() + " </td><td>" + DtMain.Rows[PageNo]["dp_dpname"].ToString() + "</td></tr></table> </td></tr>";
            flag = flag + 1;
            Format += "<tr style=\"background-color: " + GetRowColor(flag) + "\"><td>Bank Details:</td><td colspan=\"2\"><table><tr><td style=\"font-weight:bold\">Primary Ac No:</td><td>" + DtMain.Rows[PageNo]["cbd_accountnumber"].ToString() + "</td><td style=\"font-weight:bold\"> Bank Name: </td><td> " + DtMain.Rows[PageNo]["bnk_bankname"].ToString() + " </td></tr></table> </td></tr>";
            flag = flag + 1;
            Format += "<tr style=\"background-color: " + GetRowColor(flag) + "\"><td > Brokerage/Charge Setup Details:</td><td></td></tr>";
            flag = flag + 1;
            if (dtBrokerageDtl.Rows.Count > 0)
            {
                Format += "<tr style=\"background-color: " + GetRowColor(flag) + "\"><td > Segment:</td><td>" + dtBrokerageDtl.Rows[0]["seg"].ToString() + "</td></tr>";
            }
            target = dtBrokerageDtl.Select(expression);

            if (target.Length > 0)
            {
                flag = flag + 1;
                Format += "<tr style=\"background-color: " + GetRowColor(flag) + "\"><td colspan=\"2\" ><table><tr><td style=\"font-weight:bold\">Min Daily Brokerage:</td><td>Rs " + ObjConvert.getFormattedvalue(decimal.Parse(target[0][3].ToString())) + "</td><td style=\"font-weight:bold\">Min Del Brokerage/Share:</td><td>Rs " + ObjConvert.getFormattedvalue(decimal.Parse(target[0][1].ToString())) + "</td><td style=\"font-weight:bold\">Min Brokerage/Order:</td><td>Rs " + ObjConvert.getFormattedvalue(decimal.Parse(target[0][4].ToString())) + "</td></tr></table></td></tr>";

            }
            expression = "brokeragemain_customerid='" + Customerid[PageNo, 0].ToString() + "' and BrokerageDetail_brkgtype='Delivery' AND SEG='" + SEGMENTMORE1.ToString() + "'";
            target = dtBrokerageDtl.Select(expression);
            if (target.Length > 0)
            {
                flag = flag + 1;
                Format += "<tr style=\"background-color: " + GetRowColor(flag) + "\"><td colspan=\"2\"> <table><tr><td>Delivery:</td><td>" + ObjConvert.getFormattedvalue(decimal.Parse(target[0]["BrokerageDetail_Rate"].ToString())) + "%</td><td>Min</td><td>" + target[0]["BrokerageDetail_MinAmount"].ToString() + "</td></tr></table></td></tr>";

            }

            expression = "brokeragemain_customerid='" + Customerid[PageNo, 0].ToString() + "' and BrokerageDetail_brkgtype='Square-Off' AND SEG='" + SEGMENTMORE1.ToString() + "'";
            target = dtBrokerageDtl.Select(expression);
            if (target.Length > 0)
            {
                flag = flag + 1;
                Format += "<tr style=\"background-color: " + GetRowColor(flag) + "\"><td colspan=\"2\"> <table><tr><td>Square-Off:</td><td>" + ObjConvert.getFormattedvalue(decimal.Parse(target[0]["BrokerageDetail_Rate"].ToString())) + "%</td><td>Min</td><td>" + target[0]["BrokerageDetail_MinAmount"].ToString() + "</td></tr></table></td></tr>";
            }

            //Format += "<tr><td > Address:</td><td>" + DtMain.Rows[0][3].ToString() + "</td></tr>";
            flag = flag + 1;

            expression = "brokeragemain_customerid='" + Customerid[PageNo, 0].ToString() + "' and ChargeSetup_ChargeType='ST' AND SEG='" + SEGMENTMORE1.ToString() + "'";
            target = dtBrokerageDtl.Select(expression);
            if (target.Length > 0)
            {
                flag = flag + 1;
                Format += "<tr style=\"background-color: " + GetRowColor(flag) + "\"><td colspan=\"2\"> <table><tr><td>Service Tax:</td><td>" + target[0]["ChargeSetup_ChargeBasis"].ToString() + "</td>";
            }

            expression = "brokeragemain_customerid='" + Customerid[PageNo, 0].ToString() + "' and ChargeSetup_ChargeType='SX' AND SEG='" + SEGMENTMORE1.ToString() + "'";
            target = dtBrokerageDtl.Select(expression);
            if (target.Length > 0)
            {
                flag = flag + 1;
                Format += "<td>;STT:</td><td>" + target[0]["ChargeSetup_ChargeBasis"].ToString() + "</td>";
            }
            expression = "brokeragemain_customerid='" + Customerid[PageNo, 0].ToString() + "' and ChargeSetup_ChargeType='TX' AND SEG='" + SEGMENTMORE1.ToString() + "'";
            target = dtBrokerageDtl.Select(expression);
            if (target.Length > 0)
            {
                flag = flag + 1;
                Format += "<td>;Transaction Charge:</td><td>" + target[0]["ChargeSetup_ChargeBasis"].ToString() + "</td>";
            }
            expression = "brokeragemain_customerid='" + Customerid[PageNo, 0].ToString() + "' and ChargeSetup_ChargeType='SD' AND SEG='" + SEGMENTMORE1.ToString() + "'";
            target = dtBrokerageDtl.Select(expression);
            if (target.Length > 0)
            {
                flag = flag + 1;
                Format += "<td>;Stamp Duty:</td><td>" + target[0]["ChargeSetup_ChargeBasis"].ToString() + "</td>";
            }
            expression = "brokeragemain_customerid='" + Customerid[PageNo, 0].ToString() + "' and ChargeSetup_ChargeType='DM' AND SEG='" + SEGMENTMORE1.ToString() + "'";
            target = dtBrokerageDtl.Select(expression);
            if (target.Length > 0)
            {
                flag = flag + 1;
                Format += "<td>;Demat Charges:</td><td>" + target[0]["ChargeSetup_ChargeBasis"].ToString() + "</td>";
            }
            expression = "brokeragemain_customerid='" + Customerid[PageNo, 0].ToString() + "' and ChargeSetup_ChargeType='SF' AND SEG='" + SEGMENTMORE1.ToString() + "'";
            target = dtBrokerageDtl.Select(expression);
            if (target.Length > 0)
            {
                flag = flag + 1;
                Format += " <td>;SEBI:</td><td>" + target[0]["ChargeSetup_ChargeBasis"].ToString() + "</td></tr></table></td></tr>";
            }
            if (SEGMENTMORE2 != "")
            {
                if (dtBrokerageDtl.Rows.Count > 0)
                {
                    Format += "<tr style=\"background-color: " + GetRowColor(flag) + "\"><td > Segment:</td><td>" + SEGMENTMORE2.ToString() + "</td></tr>";
                }
                target = dtBrokerageDtl.Select(expression);

                if (target.Length > 0)
                {
                    flag = flag + 1;
                    Format += "<tr style=\"background-color: " + GetRowColor(flag) + "\"><td colspan=\"2\" ><table><tr><td style=\"font-weight:bold\">Min Daily Brokerage:</td><td>Rs " + ObjConvert.getFormattedvalue(decimal.Parse(target[0][3].ToString())) + "</td><td style=\"font-weight:bold\">Min Del Brokerage/Share:</td><td>Rs " + ObjConvert.getFormattedvalue(decimal.Parse(target[0][1].ToString())) + "</td><td style=\"font-weight:bold\">Min Brokerage/Order:</td><td>Rs " + ObjConvert.getFormattedvalue(decimal.Parse(target[0][4].ToString())) + "</td></tr></table></td></tr>";

                }
                expression = "brokeragemain_customerid='" + Customerid[PageNo, 0].ToString() + "' and BrokerageDetail_brkgtype='Delivery' AND SEG='" + SEGMENTMORE2.ToString() + "'";
                target = dtBrokerageDtl.Select(expression);
                if (target.Length > 0)
                {
                    flag = flag + 1;
                    Format += "<tr style=\"background-color: " + GetRowColor(flag) + "\"><td colspan=\"2\"> <table><tr><td>Delivery:</td><td>" + ObjConvert.getFormattedvalue(decimal.Parse(target[0]["BrokerageDetail_Rate"].ToString())) + "%</td><td>Min</td><td>" + target[0]["BrokerageDetail_MinAmount"].ToString() + "</td></tr></table></td></tr>";

                }

                expression = "brokeragemain_customerid='" + Customerid[PageNo, 0].ToString() + "' and BrokerageDetail_brkgtype='Square-Off' AND SEG='" + SEGMENTMORE2.ToString() + "'";
                target = dtBrokerageDtl.Select(expression);
                if (target.Length > 0)
                {
                    flag = flag + 1;
                    Format += "<tr style=\"background-color: " + GetRowColor(flag) + "\"><td colspan=\"2\"> <table><tr><td>Square-Off:</td><td>" + ObjConvert.getFormattedvalue(decimal.Parse(target[0]["BrokerageDetail_Rate"].ToString())) + "%</td><td>Min</td><td>" + target[0]["BrokerageDetail_MinAmount"].ToString() + "</td></tr></table></td></tr>";
                }

                //Format += "<tr><td > Address:</td><td>" + DtMain.Rows[0][3].ToString() + "</td></tr>";
                flag = flag + 1;

                expression = "brokeragemain_customerid='" + Customerid[PageNo, 0].ToString() + "' and ChargeSetup_ChargeType='ST' AND SEG='" + SEGMENTMORE2.ToString() + "'";
                target = dtBrokerageDtl.Select(expression);
                if (target.Length > 0)
                {
                    flag = flag + 1;
                    Format += "<tr style=\"background-color: " + GetRowColor(flag) + "\"><td colspan=\"2\"> <table><tr><td>Service Tax:</td><td>" + target[0]["ChargeSetup_ChargeBasis"].ToString() + "</td>";
                }

                expression = "brokeragemain_customerid='" + Customerid[PageNo, 0].ToString() + "' and ChargeSetup_ChargeType='SX' AND SEG='" + SEGMENTMORE2.ToString() + "'";
                target = dtBrokerageDtl.Select(expression);
                if (target.Length > 0)
                {
                    flag = flag + 1;
                    Format += "<td> ;STT:</td><td>" + target[0]["ChargeSetup_ChargeBasis"].ToString() + "</td>";
                }
                expression = "brokeragemain_customerid='" + Customerid[PageNo, 0].ToString() + "' and ChargeSetup_ChargeType='TX' AND SEG='" + SEGMENTMORE2.ToString() + "'";
                target = dtBrokerageDtl.Select(expression);
                if (target.Length > 0)
                {
                    flag = flag + 1;
                    Format += "<td>Transaction Charge:</td><td>" + target[0]["ChargeSetup_ChargeBasis"].ToString() + "</td>";
                }
                expression = "brokeragemain_customerid='" + Customerid[PageNo, 0].ToString() + "' and ChargeSetup_ChargeType='SD' AND SEG='" + SEGMENTMORE2.ToString() + "'";
                target = dtBrokerageDtl.Select(expression);
                if (target.Length > 0)
                {
                    flag = flag + 1;
                    Format += "<td>;Stamp Duty:</td><td>" + target[0]["ChargeSetup_ChargeBasis"].ToString() + "</td>";
                }
                expression = "brokeragemain_customerid='" + Customerid[PageNo, 0].ToString() + "' and ChargeSetup_ChargeType='DM' AND SEG='" + SEGMENTMORE2.ToString() + "'";
                target = dtBrokerageDtl.Select(expression);
                if (target.Length > 0)
                {
                    flag = flag + 1;
                    Format += "<td>;Demat Charges:</td><td>" + target[0]["ChargeSetup_ChargeBasis"].ToString() + "</td>";
                }
                expression = "brokeragemain_customerid='" + Customerid[PageNo, 0].ToString() + "' and ChargeSetup_ChargeType='SF' AND SEG='" + SEGMENTMORE2.ToString() + "'";
                target = dtBrokerageDtl.Select(expression);
                if (target.Length > 0)
                {
                    flag = flag + 1;
                    Format += "<td>;SEBI:</td><td>" + target[0]["ChargeSetup_ChargeBasis"].ToString() + "</td></tr></table></td></tr>";
                }
            }
            Format += "<tr style=\"background-color: " + GetRowColor(flag) + "\"><td><script>height();</script></td></tr></table>";
            MainContainer.InnerHtml = Format;
            Session["MainDataTable"] = DtMain;
            HdPageNo.Value = PageNo.ToString();
            if (DtMain.Rows.Count == 1)
            {
                trButton.Visible = false;
            }
            else
            {

            }
        }
        private void FillDataTable()
        {

        }
        protected string GetRowColor(int i)
        {
            if (i++ % 2 == 0)
                return "#fff0f5";
            else
                return "White";
        }




        public void CreateDataTable()
        {
            if (rdAll.Checked == true)
            {

                DtMain = oDBEngine.GetDataTable("(Select cnt_ucc,isnull(ltrim(rtrim(cnt_firstname)),'')+' '+isnull(cnt_middlename,'')+' '+isnull(cnt_lastname,'')as Name,cnt_clienttype, tbl_address.*,tbl_phone.*,isnull((select eml_email from tbl_master_email where tbl_master_email.eml_type='Official' and eml_cntId=tbl_master_contact.cnt_internalid),'') as email,isnull((select gpm_description from tbl_master_groupmaster where gpm_id in(select top 1 grp_groupmaster  from tbl_trans_group where grp_grouptype='Family' and grp_contactid=tbl_master_contact.cnt_internalid)),'') as Group1,isnull((select crg_number  from tbl_master_contactregistration where crg_type='Pancard' and crg_cntid=tbl_master_contact.cnt_internalid),'')as pan,tbl_bank.cbd_accountnumber,tbl_bank.bnk_bankname,tbl_dp.dpd_clientid,tbl_dp.dpd_dpcode,tbl_dp.dp_dpname,(select lgl_legalstatus from tbl_master_legalstatus where lgl_id=tbl_master_contact.cnt_legalStatus)as status from (select isnull(a.add_address1,b.add_address1)as address,isnull(a.city,b.city) as city,isnull(a.add_pin,b.add_pin)as pin,isnull(a.state,b.state) as state,isnull(a.add_cntid,b.add_cntid) as internalid from(select top 1 add_address1,add_cntid,(select city_name from tbl_master_city where city_id=tbl_master_address.add_city)as city,add_pin,(select state from tbl_master_state where id=tbl_master_address.add_state)as state from tbl_master_address where add_addresstype='Residence')as a full outer join (select top 1 add_address1,add_cntid,(select city_name from tbl_master_city where city_id=tbl_master_address.add_city)as city,add_pin,(select state from tbl_master_state where id=tbl_master_address.add_state)as state from tbl_master_address where add_addresstype='Office')as b on a.add_cntid=b.add_cntid) as tbl_address right outer join tbl_master_contact on tbl_address.internalid=tbl_master_contact.cnt_internalid left outer join (select isnull(c.landno,d.landno)as LandOrMobile,isnull(c.phf_cntid,d.phf_cntid)as phf_cntid from(select phf_phonenumber,phf_type,phf_cntid,isnull(phf_countrycode,'')+isnull(phf_areacode,'')+isnull(phf_phonenumber,'') as landno from tbl_master_phonefax where phf_type='Mobile')as c full outer join (select phf_phonenumber,phf_type,phf_cntid,isnull(phf_countrycode,'')+isnull(phf_areacode,'')+isnull(phf_phonenumber,'') as landno from tbl_master_phonefax where phf_type='Office')as d on c.phf_cntid=d.phf_cntid) as tbl_phone on tbl_phone.phf_cntid=tbl_master_contact.cnt_internalid left outer join (select cbd_accountnumber,bnk_bankname,cbd_cntid from tbl_master_bank left outer join tbl_trans_contactbankdetails on bnk_id=cbd_bankcode where cbd_accountcategory='Primary')as tbl_bank on tbl_bank.cbd_cntid=tbl_master_contact.cnt_internalid left outer join ( select e.dpd_clientid,e.dpd_dpcode,f.dp_dpname,e.dpd_cntid from tbl_master_contactdpdetails as e inner join tbl_master_depositoryParticipants as f on e.dpd_dpcode=f.dp_dpid) as tbl_dp on tbl_dp.dpd_cntid=tbl_master_contact.cnt_internalid) as a", "a.*", null, " a.name");
                DataColumn DtIdentity = new DataColumn();
                DtIdentity.AllowDBNull = false;
                DtIdentity.ColumnName = "Auto_Id";
                DtIdentity.DataType = System.Type.GetType("System.Int32");
                DtMain.Columns.Add(DtIdentity);
                if (DtMain.Rows.Count > 0)
                {
                    for (int i = 0; i < DtMain.Rows.Count; i++)
                    {
                        DtMain.Rows[i]["Auto_Id"] = i;
                    }
                }
            }
            Session["MainDataTable"] = DtMain;
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
        }
        protected void btnPrevious_Click1(object sender, ImageClickEventArgs e)
        {
            PageNo = int.Parse(HdPageNo.Value.ToString()) - 1;
            DtMain = (DataTable)Session["MainDataTable"];
            fillHTML();
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
        }
        protected void btnLast_Click1(object sender, ImageClickEventArgs e)
        {
            btnNext.Visible = false;
            btnFirst.Visible = true;
            btnPrevious.Visible = true;
            DtMain = (DataTable)Session["MainDataTable"];
            PageNo = int.Parse(DtMain.Rows.Count.ToString()) - 1;
            fillHTML();
        }
        protected void btnReportShow_Click(object sender, EventArgs e)
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]))
            {
                SqlCommand cmd = new SqlCommand();

                cmd.Connection = con;
                cmd.CommandText = "[Contact_Details]";

                cmd.CommandType = CommandType.StoredProcedure;
                if (Session["KeyValSegment"] == null)
                {
                    Page.ClientScript.RegisterStartupScript(GetType(), "message", "<script='JavaScript'>alert('Select a client')</script>");
                }
                else
                {
                    cmd.Parameters.AddWithValue("@SELINTERNALID", Session["KeyValSegment"].ToString());

                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = cmd;
                    cmd.CommandTimeout = 0;
                    ds.Reset();
                    da.Fill(ds);
                    da.Dispose();

                    ReportDocument report = new ReportDocument();
                    //ds.Tables[0].WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\ContactDetails.xsd");
                    string tmpPdfPath = string.Empty;
                    tmpPdfPath = HttpContext.Current.Server.MapPath("..\\Reports\\rpt_Contact_Details.rpt");
                    report.Load(tmpPdfPath);
                    report.SetDataSource(ds.Tables[0]);
                    report.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, HttpContext.Current.Response, true, "Contact_Details");

                    report.Dispose();
                    GC.Collect();
                }
            }
        }
    }
}