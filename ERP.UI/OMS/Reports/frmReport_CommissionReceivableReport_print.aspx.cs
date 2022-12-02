using System;
using System.Data;
using System.Web;
using BusinessLogicLayer;

namespace ERP.OMS.Reports
{
    public partial class Reports_frmReport_CommissionReceivableReport_print : System.Web.UI.Page
    {
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        BusinessLogicLayer.Converter oconverter = new BusinessLogicLayer.Converter();
        protected void Page_Load(object sender, EventArgs e)
        {
            lblFromDate.Text = Request.QueryString["Fdate"].ToString().Split('/')[1] + "-" + Request.QueryString["Fdate"].ToString().Split('/')[0] + "-" + Request.QueryString["Fdate"].ToString().Split('/')[2];
            lblToDate.Text = Request.QueryString["Tdate"].ToString().Split('/')[1] + "-" + Request.QueryString["Tdate"].ToString().Split('/')[0] + "-" + Request.QueryString["Tdate"].ToString().Split('/')[2];

            DataTable DT = oDBEngine.GetDataTable(" (select *,(select dbo.Insurance_CommissionCalculation(trn_Scheme,trn_transMode,trn_BusinessClass,trn_paymentAmt,'BA',trn_PreminumAmt,'Base',issueDate,trn_PaymentDate )) as BaseCommission,(select dbo.Insurance_CommissionCalculation(trn_Scheme,trn_transMode,trn_BusinessClass,trn_paymentAmt,'OR',trn_PreminumAmt,'Base',issueDate,trn_PaymentDate )) as ORCCommission,(select dbo.Insurance_CommissionCalculation(trn_Scheme,trn_transMode,trn_BusinessClass,trn_paymentAmt,'TopUp',trn_PreminumAmt,'Base',issueDate,trn_PaymentDate )) as TopUpCommission  " +
                " from ((select trn_id,(select insu_nameOfCompany from tbl_master_insurerName where insu_internalId in(select prd_insurerName from tbl_master_productsDetails where prd_internalID=trn_scheme )) as company " +
                " ,(select prds_description from tbl_master_products where prds_internalID=trn_scheme) as Product " +
                " ,(select isnull(RTRIM(cnt_firstName),'')+' '+isnull(RTRIM(cnt_middleName),'')+' '+isnull(RTRIM(cnt_lastName),'')+' ['+isnull(RTRIM(cnt_UCC),'')+']' from tbl_master_contact where cnt_internalid=trn_ContactID) as client" +
                " ,(select prd_insurerName from tbl_master_productsDetails where prd_internalID=trn_scheme ) companyID,trn_TransNo,trn_PolicyNo,trn_AppNo,trn_Scheme,trn_transMode,trn_BusinessClass,trn_ContactID,(case trn_BusinessClass when 'I' then 'Individual-Resident' when 'C' then 'Corporate' when 'T' then 'Trust' when 'N' then 'Individual-Non Resident' when 'A' then 'Any' else 'N/A' end) as BusCalss,convert(varchar,trn_LoginDate,5) as trn_LoginDate,convert(varchar,trn_IssueDate,5) as trn_IssueDate,trn_IssueDate as issueDate,'B' as type " +
                " from tbl_trans_insmain ,tbl_master_user,tbl_master_contact" +
                " where trn_id not in (select A.trn_id from tbl_trans_insmain A where dateadd(mm,12,dateadd(dd,trn_FreeLookupPeriod,trn_IssueDate)) <= '" + Request.QueryString["Fdate"].ToString() + "' ) " +
                " and trn_status=4  and tbl_trans_insmain.CreateUser=tbl_master_user.user_id and tbl_master_user.user_contactID=tbl_master_contact.cnt_internalID and tbl_master_contact.cnt_branchid in (" + HttpContext.Current.Session["userbranchHierarchy"] + ")" +
                " ) as A inner join" +
                " (select trn_id AS TT,trn_PreminumAmt,trn_paymentAmt,trn_transNo AS TTT,trn_PaymentDate from tbl_trans_insdetail " +
                " where  cast(trn_PaymentDate as datetime) between cast('" + Request.QueryString["Fdate"].ToString() + "' as datetime) and cast('" + Request.QueryString["Tdate"].ToString() + "' as datetime) and trn_PaymentAmt is not NULL" +
                " ) as B on A.trn_TransNo=B.TTT) ) as D" +
                " Union (select *,(select dbo.Insurance_CommissionCalculation(trn_Scheme,trn_transMode,trn_BusinessClass,trn_paymentAmt,'BA',trn_PreminumAmt,'Trail',issueDate,trn_PaymentDate )) as BaseCommission,'0' as ORCCommission ,(select dbo.Insurance_CommissionCalculation(trn_Scheme,trn_transMode,trn_BusinessClass,trn_paymentAmt,'TopUp',trn_PreminumAmt,'Trail',issueDate,trn_PaymentDate  )) as TopUpCommission " +
                " from ((select trn_id,(select insu_nameOfCompany from tbl_master_insurerName where insu_internalId in(select prd_insurerName from tbl_master_productsDetails where prd_internalID=trn_scheme )) as company " +
                " ,(select prds_description from tbl_master_products where prds_internalID=trn_scheme) as Product " +
                " ,(select isnull(RTRIM(cnt_firstName),'')+' '+isnull(RTRIM(cnt_middleName),'')+' '+isnull(RTRIM(cnt_lastName),'')+' ['+isnull(RTRIM(cnt_UCC),'')+']' from tbl_master_contact where cnt_internalid=trn_ContactID) as client" +
                " ,(select prd_insurerName from tbl_master_productsDetails where prd_internalID=trn_scheme ) companyID,trn_TransNo,trn_AppNo,trn_PolicyNo,trn_Scheme,trn_transMode,trn_BusinessClass,trn_ContactID,(case trn_BusinessClass when 'I' then 'Individual-Resident' when 'C' then 'Corporate' when 'T' then 'Trust' when 'N' then 'Individual-Non Resident' when 'A' then 'Any' else 'N/A' end) as BusCalss,convert(varchar,trn_LoginDate,5) as trn_LoginDate,convert(varchar,trn_IssueDate,5) as trn_IssueDate,trn_IssueDate as issueDate,'T' as type" +
                " from tbl_trans_insmain ,tbl_master_user,tbl_master_contact" +
                " where trn_id  in (select A.trn_id from tbl_trans_insmain A where dateadd(mm,12,dateadd(dd,trn_FreeLookupPeriod,trn_IssueDate)) < '" + Request.QueryString["Fdate"].ToString() + "' ) " +
                " and trn_status=4  and tbl_trans_insmain.CreateUser=tbl_master_user.user_id and tbl_master_user.user_contactID=tbl_master_contact.cnt_internalID and tbl_master_contact.cnt_branchid in (" + HttpContext.Current.Session["userbranchHierarchy"] + ")" +
                " ) as A inner join (select trn_id AS TT,trn_PreminumAmt,trn_paymentAmt,trn_transNo AS TTT,trn_PaymentDate from tbl_trans_insdetail " +
                " where  cast(trn_PaymentDate as datetime) between cast('" + Request.QueryString["Fdate"].ToString() + "' as datetime) and cast('" + Request.QueryString["Tdate"].ToString() + "' as datetime) and trn_PaymentAmt is not NULL" +
                " ) as B on A.trn_TransNo=B.TTT) ) ",
                 " *  "
                 , null, "  D.company,D.product,D.client,D.type");
            string Company = "";
            string Product = "";
            string clients = "";
            //GRAND TOTAL VARIABLE
            decimal GT_PremiumAmt = 0;
            decimal GT_PremPaidAmt = 0;
            decimal GT_BaseComm = 0;
            decimal GT_ORCcomm = 0;
            decimal GT_Trail = 0;
            decimal GT_TopUp = 0;
            decimal GT_Total = 0;
            //PRODUCT TOTAL VARIABLE
            decimal STP_PremiumAmt = 0;
            decimal STP_PremPaidAmt = 0;
            decimal STP_BaseComm = 0;
            decimal STP_ORCcomm = 0;
            decimal STP_Trail = 0;
            decimal STP_TopUp = 0;
            decimal STP_Total = 0;
            //COMPANY TOTAL VARIABLE
            decimal STC_PremiumAmt = 0;
            decimal STC_PremPaidAmt = 0;
            decimal STC_BaseComm = 0;
            decimal STC_ORCcomm = 0;
            decimal STC_Trail = 0;
            decimal STC_TopUp = 0;
            decimal STC_Total = 0;

            string HTML = "<table border=\"1px\" cellpadding=\"2\" cellspacing=\"0\" style=\"border:solid 1px blue; font-size:7px\"> <tr>" +
                    "<td><strong>Client name [Code]</strong></td><td><strong>Policy No.</strong></td>" +
                    "<td><strong>Application No.</strong></td><td><strong>Busin. Class</strong></td>" +
                    "<td><strong>Login Date</strong></td><td><strong>Issued date</strong></td>" +
                    "<td><strong>Premium Amt.</strong></td><td><strong>Prm. Paid</strong></td>" +
                    "<td><strong>Base Comm.</strong></td><td><strong>ORC Comm.</strong></td>" +
                    "<td><strong>Trail Comm.</strong></td><td><strong>Top-Up Comm.</strong></td>" +
                    "<td><strong>Total</strong></td><td><strong>Total Recvd.</strong></td></tr>";

            if (DT.Rows.Count > 0)
            {
                for (int i = 0; i < DT.Rows.Count; i++)
                {
                    if (i == 0)
                    {
                        Company = DT.Rows[i]["companyID"].ToString();
                        Product = DT.Rows[i]["trn_Scheme"].ToString();
                        clients = DT.Rows[i]["trn_ContactID"].ToString();
                        HTML += "<tr><td colspan=\"14\" style=\"text-align:left; padding-left:1px\">" + DT.Rows[i]["company"].ToString() + "</td></tr>";
                        HTML += "<tr><td colspan=\"14\" style=\"text-align:left; padding-left:2px\">" + DT.Rows[i]["Product"].ToString() + "</td></tr>";
                        HTML += "<tr>";
                        HTML += "<td style=\"text-align:left; padding-left:3px\">" + DT.Rows[i]["client"].ToString() + "</td>";
                        HTML += "<td style=\"text-align:left;\">" + DT.Rows[i]["trn_PolicyNo"].ToString() + "</td>";
                        HTML += "<td style=\"text-align:left;\">" + DT.Rows[i]["trn_AppNo"].ToString() + "</td>";
                        HTML += "<td style=\"text-align:left;\">" + DT.Rows[i]["BusCalss"].ToString() + "</td>";
                        if (DT.Rows[i]["trn_LoginDate"] != DBNull.Value)
                            HTML += "<td style=\"text-align:left;\">" + DT.Rows[i]["trn_LoginDate"].ToString() + "</td>";
                        else
                            HTML += "<td style=\"text-align:left;\">&nbsp;</td>";
                        HTML += "<td style=\"text-align:left;\">" + DT.Rows[i]["trn_IssueDate"].ToString() + "</td>";
                        HTML += "<td style=\"text-align:right;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(decimal.Parse(DT.Rows[i]["trn_PreminumAmt"].ToString())) + "</td>";
                        HTML += "<td style=\"text-align:right;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(decimal.Parse(DT.Rows[i]["trn_paymentAmt"].ToString())) + "</td>";
                        if (DT.Rows[i]["type"].ToString() == "B")
                        {
                            HTML += "<td style=\"text-align:right;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(decimal.Parse(DT.Rows[i]["BaseCommission"].ToString())) + "</td>";
                            HTML += "<td style=\"text-align:right;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(decimal.Parse(DT.Rows[i]["ORCCommission"].ToString())) + "</td>";
                            HTML += "<td style=\"text-align:right;\">&nbsp;</td>";
                            STP_BaseComm += decimal.Parse(DT.Rows[i]["BaseCommission"].ToString());
                            GT_BaseComm += decimal.Parse(DT.Rows[i]["BaseCommission"].ToString());
                            STC_BaseComm += decimal.Parse(DT.Rows[i]["BaseCommission"].ToString());
                        }
                        else
                        {
                            HTML += "<td style=\"text-align:right;\">&nbsp;</td>";
                            HTML += "<td style=\"text-align:right;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(decimal.Parse(DT.Rows[i]["ORCCommission"].ToString())) + "</td>";
                            HTML += "<td style=\"text-align:right;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(decimal.Parse(DT.Rows[i]["BaseCommission"].ToString())) + "</td>";
                            STP_Trail += decimal.Parse(DT.Rows[i]["BaseCommission"].ToString());
                            GT_Trail += decimal.Parse(DT.Rows[i]["BaseCommission"].ToString());
                            STC_Trail += decimal.Parse(DT.Rows[i]["BaseCommission"].ToString());
                        }
                        HTML += "<td style=\"text-align:right;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(decimal.Parse(DT.Rows[i]["TopUpCommission"].ToString())) + "</td>";

                        decimal tot = decimal.Parse(DT.Rows[i]["BaseCommission"].ToString()) + decimal.Parse(DT.Rows[i]["ORCCommission"].ToString()) + decimal.Parse(DT.Rows[i]["TopUpCommission"].ToString());
                        HTML += "<td style=\"text-align:right;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(tot) + "</td>";
                        HTML += "<td style=\"text-align:right;\">&nbsp;</td>";
                        HTML += "</tr>";
                        STP_PremiumAmt += decimal.Parse(DT.Rows[i]["trn_PreminumAmt"].ToString());
                        STP_PremPaidAmt += decimal.Parse(DT.Rows[i]["trn_paymentAmt"].ToString());
                        STP_ORCcomm += decimal.Parse(DT.Rows[i]["ORCCommission"].ToString());
                        STP_TopUp += decimal.Parse(DT.Rows[i]["TopUpCommission"].ToString());
                        STP_Total += tot;

                        GT_PremiumAmt += decimal.Parse(DT.Rows[i]["trn_PreminumAmt"].ToString());
                        GT_PremPaidAmt += decimal.Parse(DT.Rows[i]["trn_paymentAmt"].ToString());
                        GT_ORCcomm += decimal.Parse(DT.Rows[i]["ORCCommission"].ToString());
                        GT_TopUp += decimal.Parse(DT.Rows[i]["TopUpCommission"].ToString());
                        GT_Total += tot;

                        STC_PremiumAmt += decimal.Parse(DT.Rows[i]["trn_PreminumAmt"].ToString());
                        STC_PremPaidAmt += decimal.Parse(DT.Rows[i]["trn_paymentAmt"].ToString());
                        STC_ORCcomm += decimal.Parse(DT.Rows[i]["ORCCommission"].ToString());
                        STC_TopUp += decimal.Parse(DT.Rows[i]["TopUpCommission"].ToString());
                        STC_Total += tot;
                    }
                    else
                    {
                        if (Company == DT.Rows[i]["companyID"].ToString())
                        {
                            if (Product == DT.Rows[i]["trn_Scheme"].ToString())
                            {
                                if (clients == DT.Rows[i]["trn_ContactID"].ToString())
                                {
                                    HTML += "<tr>";
                                    HTML += "<td style=\"text-align:left; padding-left:3px\"></td>";
                                    HTML += "<td style=\"text-align:left;\">" + DT.Rows[i]["trn_PolicyNo"].ToString() + "</td>";
                                    HTML += "<td style=\"text-align:left;\">" + DT.Rows[i]["trn_AppNo"].ToString() + "</td>";
                                    HTML += "<td style=\"text-align:left;\">" + DT.Rows[i]["BusCalss"].ToString() + "</td>";
                                    if (DT.Rows[i]["trn_LoginDate"] != DBNull.Value)
                                        HTML += "<td style=\"text-align:left;\">" + DT.Rows[i]["trn_LoginDate"].ToString() + "</td>";
                                    else
                                        HTML += "<td style=\"text-align:left;\">&nbsp;</td>";
                                    HTML += "<td style=\"text-align:left;\">" + DT.Rows[i]["trn_IssueDate"].ToString() + "</td>";
                                    HTML += "<td style=\"text-align:right;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(decimal.Parse(DT.Rows[i]["trn_PreminumAmt"].ToString())) + "</td>";
                                    HTML += "<td style=\"text-align:right;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(decimal.Parse(DT.Rows[i]["trn_paymentAmt"].ToString())) + "</td>";
                                    if (DT.Rows[i]["type"].ToString() == "B")
                                    {
                                        HTML += "<td style=\"text-align:right;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(decimal.Parse(DT.Rows[i]["BaseCommission"].ToString())) + "</td>";
                                        HTML += "<td style=\"text-align:right;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(decimal.Parse(DT.Rows[i]["ORCCommission"].ToString())) + "</td>";
                                        HTML += "<td style=\"text-align:right;\">&nbsp;</td>";
                                        STP_BaseComm += decimal.Parse(DT.Rows[i]["BaseCommission"].ToString());
                                        GT_BaseComm += decimal.Parse(DT.Rows[i]["BaseCommission"].ToString());
                                        STC_BaseComm += decimal.Parse(DT.Rows[i]["BaseCommission"].ToString());
                                    }
                                    else
                                    {
                                        HTML += "<td style=\"text-align:right;\">&nbsp;</td>";
                                        HTML += "<td style=\"text-align:right;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(decimal.Parse(DT.Rows[i]["ORCCommission"].ToString())) + "</td>";
                                        HTML += "<td style=\"text-align:right;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(decimal.Parse(DT.Rows[i]["BaseCommission"].ToString())) + "</td>";
                                        STP_Trail += decimal.Parse(DT.Rows[i]["BaseCommission"].ToString());
                                        GT_Trail += decimal.Parse(DT.Rows[i]["BaseCommission"].ToString());
                                        STC_Trail += decimal.Parse(DT.Rows[i]["BaseCommission"].ToString());
                                    }
                                    HTML += "<td style=\"text-align:right;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(decimal.Parse(DT.Rows[i]["TopUpCommission"].ToString())) + "</td>";

                                    decimal tot = decimal.Parse(DT.Rows[i]["BaseCommission"].ToString()) + decimal.Parse(DT.Rows[i]["ORCCommission"].ToString()) + decimal.Parse(DT.Rows[i]["TopUpCommission"].ToString());
                                    HTML += "<td style=\"text-align:right;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(tot) + "</td>";
                                    HTML += "<td style=\"text-align:right;\">&nbsp;</td>";
                                    HTML += "</tr>";
                                    STP_PremiumAmt += decimal.Parse(DT.Rows[i]["trn_PreminumAmt"].ToString());
                                    STP_PremPaidAmt += decimal.Parse(DT.Rows[i]["trn_paymentAmt"].ToString());
                                    //STP_BaseComm += decimal.Parse(DT.Rows[i]["BaseCommission"].ToString());
                                    STP_ORCcomm += decimal.Parse(DT.Rows[i]["ORCCommission"].ToString());
                                    STP_TopUp += decimal.Parse(DT.Rows[i]["TopUpCommission"].ToString());
                                    STP_Total += tot;

                                    GT_PremiumAmt += decimal.Parse(DT.Rows[i]["trn_PreminumAmt"].ToString());
                                    GT_PremPaidAmt += decimal.Parse(DT.Rows[i]["trn_paymentAmt"].ToString());
                                    //GT_BaseComm += decimal.Parse(DT.Rows[i]["BaseCommission"].ToString());
                                    GT_ORCcomm += decimal.Parse(DT.Rows[i]["ORCCommission"].ToString());
                                    GT_TopUp += decimal.Parse(DT.Rows[i]["TopUpCommission"].ToString());
                                    GT_Total += tot;

                                    STC_PremiumAmt += decimal.Parse(DT.Rows[i]["trn_PreminumAmt"].ToString());
                                    STC_PremPaidAmt += decimal.Parse(DT.Rows[i]["trn_paymentAmt"].ToString());
                                    //STC_BaseComm += decimal.Parse(DT.Rows[i]["BaseCommission"].ToString());
                                    STC_ORCcomm += decimal.Parse(DT.Rows[i]["ORCCommission"].ToString());
                                    STC_TopUp += decimal.Parse(DT.Rows[i]["TopUpCommission"].ToString());
                                    STC_Total += tot;
                                }
                                else
                                {
                                    clients = DT.Rows[i]["trn_ContactID"].ToString();
                                    HTML += "<tr>";
                                    HTML += "<td style=\"text-align:left; padding-left:3px\">" + DT.Rows[i]["client"].ToString() + "</td>";
                                    HTML += "<td style=\"text-align:left;\">" + DT.Rows[i]["trn_PolicyNo"].ToString() + "</td>";
                                    HTML += "<td style=\"text-align:left;\">" + DT.Rows[i]["trn_AppNo"].ToString() + "</td>";
                                    HTML += "<td style=\"text-align:left;\">" + DT.Rows[i]["BusCalss"].ToString() + "</td>";
                                    if (DT.Rows[i]["trn_LoginDate"] != DBNull.Value)
                                        HTML += "<td style=\"text-align:left;\">" + DT.Rows[i]["trn_LoginDate"].ToString() + "</td>";
                                    else
                                        HTML += "<td style=\"text-align:left;\">&nbsp;</td>";
                                    HTML += "<td style=\"text-align:left;\">" + DT.Rows[i]["trn_IssueDate"].ToString() + "</td>";
                                    HTML += "<td style=\"text-align:right;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(decimal.Parse(DT.Rows[i]["trn_PreminumAmt"].ToString())) + "</td>";
                                    HTML += "<td style=\"text-align:right;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(decimal.Parse(DT.Rows[i]["trn_paymentAmt"].ToString())) + "</td>";
                                    if (DT.Rows[i]["type"].ToString() == "B")
                                    {
                                        HTML += "<td style=\"text-align:right;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(decimal.Parse(DT.Rows[i]["BaseCommission"].ToString())) + "</td>";
                                        HTML += "<td style=\"text-align:right;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(decimal.Parse(DT.Rows[i]["ORCCommission"].ToString())) + "</td>";
                                        HTML += "<td style=\"text-align:right;\">&nbsp;</td>";
                                        STP_BaseComm += decimal.Parse(DT.Rows[i]["BaseCommission"].ToString());
                                        GT_BaseComm += decimal.Parse(DT.Rows[i]["BaseCommission"].ToString());
                                        STC_BaseComm += decimal.Parse(DT.Rows[i]["BaseCommission"].ToString());
                                    }
                                    else
                                    {
                                        HTML += "<td style=\"text-align:right;\">&nbsp;</td>";
                                        HTML += "<td style=\"text-align:right;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(decimal.Parse(DT.Rows[i]["ORCCommission"].ToString())) + "</td>";
                                        HTML += "<td style=\"text-align:right;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(decimal.Parse(DT.Rows[i]["BaseCommission"].ToString())) + "</td>";
                                        STP_Trail += decimal.Parse(DT.Rows[i]["BaseCommission"].ToString());
                                        GT_Trail += decimal.Parse(DT.Rows[i]["BaseCommission"].ToString());
                                        STC_Trail += decimal.Parse(DT.Rows[i]["BaseCommission"].ToString());
                                    }
                                    HTML += "<td style=\"text-align:right;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(decimal.Parse(DT.Rows[i]["TopUpCommission"].ToString())) + "</td>";

                                    decimal tot = decimal.Parse(DT.Rows[i]["BaseCommission"].ToString()) + decimal.Parse(DT.Rows[i]["ORCCommission"].ToString()) + decimal.Parse(DT.Rows[i]["TopUpCommission"].ToString());
                                    HTML += "<td style=\"text-align:right;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(tot) + "</td>";
                                    HTML += "<td style=\"text-align:right;\">&nbsp;</td>";
                                    HTML += "</tr>";

                                    STP_PremiumAmt += decimal.Parse(DT.Rows[i]["trn_PreminumAmt"].ToString());
                                    STP_PremPaidAmt += decimal.Parse(DT.Rows[i]["trn_paymentAmt"].ToString());
                                    //STP_BaseComm += decimal.Parse(DT.Rows[i]["BaseCommission"].ToString());
                                    STP_ORCcomm += decimal.Parse(DT.Rows[i]["ORCCommission"].ToString());
                                    STP_TopUp += decimal.Parse(DT.Rows[i]["TopUpCommission"].ToString());
                                    STP_Total += tot;

                                    GT_PremiumAmt += decimal.Parse(DT.Rows[i]["trn_PreminumAmt"].ToString());
                                    GT_PremPaidAmt += decimal.Parse(DT.Rows[i]["trn_paymentAmt"].ToString());
                                    //GT_BaseComm += decimal.Parse(DT.Rows[i]["BaseCommission"].ToString());
                                    GT_ORCcomm += decimal.Parse(DT.Rows[i]["ORCCommission"].ToString());
                                    GT_TopUp += decimal.Parse(DT.Rows[i]["TopUpCommission"].ToString());
                                    GT_Total += tot;

                                    STC_PremiumAmt += decimal.Parse(DT.Rows[i]["trn_PreminumAmt"].ToString());
                                    STC_PremPaidAmt += decimal.Parse(DT.Rows[i]["trn_paymentAmt"].ToString());
                                    //STC_BaseComm += decimal.Parse(DT.Rows[i]["BaseCommission"].ToString());
                                    STC_ORCcomm += decimal.Parse(DT.Rows[i]["ORCCommission"].ToString());
                                    STC_TopUp += decimal.Parse(DT.Rows[i]["TopUpCommission"].ToString());
                                    STC_Total += tot;
                                }
                            }
                            else
                            {
                                //product change so print the total of all the cunstomers data here first
                                //Sub-Total for Product

                                HTML += "<tr>";
                                HTML += "<td style=\"text-align:left; padding-left:4px\" colspan=\"6\">Product Total</td>";

                                HTML += "<td style=\"text-align:right;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(STP_PremiumAmt) + "</td>";
                                HTML += "<td style=\"text-align:right;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(STP_PremPaidAmt) + "</td>";
                                HTML += "<td style=\"text-align:right;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(STP_BaseComm) + "</td>";
                                HTML += "<td style=\"text-align:right;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(STP_ORCcomm) + "</td>";
                                HTML += "<td style=\"text-align:right;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(STP_Trail) + "</td>";
                                HTML += "<td style=\"text-align:right;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(STP_TopUp) + "</td>";

                                HTML += "<td style=\"text-align:right;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(STP_Total) + "</td>";
                                HTML += "<td style=\"text-align:right;\">&nbsp;</td>";
                                HTML += "</tr>";

                                STP_PremiumAmt = 0;
                                STP_PremPaidAmt = 0;
                                STP_BaseComm = 0;
                                STP_ORCcomm = 0;
                                STP_TopUp = 0;
                                STP_Total = 0;
                                //---end
                                Product = DT.Rows[i]["trn_Scheme"].ToString();
                                clients = DT.Rows[i]["trn_ContactID"].ToString();
                                HTML += "<tr><td colspan=\"14\" style=\"text-align:left; padding-left:2px\">" + DT.Rows[i]["Product"].ToString() + "</td></tr>";

                                HTML += "<tr>";
                                HTML += "<td style=\"text-align:left; padding-left:3px\">" + DT.Rows[i]["client"].ToString() + "</td>";
                                HTML += "<td style=\"text-align:left;\">" + DT.Rows[i]["trn_PolicyNo"].ToString() + "</td>";
                                HTML += "<td style=\"text-align:left;\">" + DT.Rows[i]["trn_AppNo"].ToString() + "</td>";
                                HTML += "<td style=\"text-align:left;\">" + DT.Rows[i]["BusCalss"].ToString() + "</td>";
                                if (DT.Rows[i]["trn_LoginDate"] != DBNull.Value)
                                    HTML += "<td style=\"text-align:left;\">" + DT.Rows[i]["trn_LoginDate"].ToString() + "</td>";
                                else
                                    HTML += "<td style=\"text-align:left;\">&nbsp;</td>";
                                HTML += "<td style=\"text-align:left;\">" + DT.Rows[i]["trn_IssueDate"].ToString() + "</td>";
                                HTML += "<td style=\"text-align:right;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(decimal.Parse(DT.Rows[i]["trn_PreminumAmt"].ToString())) + "</td>";
                                HTML += "<td style=\"text-align:right;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(decimal.Parse(DT.Rows[i]["trn_paymentAmt"].ToString())) + "</td>";
                                if (DT.Rows[i]["type"].ToString() == "B")
                                {
                                    HTML += "<td style=\"text-align:right;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(decimal.Parse(DT.Rows[i]["BaseCommission"].ToString())) + "</td>";
                                    HTML += "<td style=\"text-align:right;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(decimal.Parse(DT.Rows[i]["ORCCommission"].ToString())) + "</td>";
                                    HTML += "<td style=\"text-align:right;\">&nbsp;</td>";
                                    STP_BaseComm += decimal.Parse(DT.Rows[i]["BaseCommission"].ToString());
                                    GT_BaseComm += decimal.Parse(DT.Rows[i]["BaseCommission"].ToString());
                                    STC_BaseComm += decimal.Parse(DT.Rows[i]["BaseCommission"].ToString());
                                }
                                else
                                {
                                    HTML += "<td style=\"text-align:right;\">&nbsp;</td>";
                                    HTML += "<td style=\"text-align:right;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(decimal.Parse(DT.Rows[i]["ORCCommission"].ToString())) + "</td>";
                                    HTML += "<td style=\"text-align:right;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(decimal.Parse(DT.Rows[i]["BaseCommission"].ToString())) + "</td>";
                                    STP_Trail += decimal.Parse(DT.Rows[i]["BaseCommission"].ToString());
                                    GT_Trail += decimal.Parse(DT.Rows[i]["BaseCommission"].ToString());
                                    STC_Trail += decimal.Parse(DT.Rows[i]["BaseCommission"].ToString());
                                }
                                HTML += "<td style=\"text-align:right;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(decimal.Parse(DT.Rows[i]["TopUpCommission"].ToString())) + "</td>";

                                decimal tot = decimal.Parse(DT.Rows[i]["BaseCommission"].ToString()) + decimal.Parse(DT.Rows[i]["ORCCommission"].ToString()) + decimal.Parse(DT.Rows[i]["TopUpCommission"].ToString());
                                HTML += "<td style=\"text-align:right;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(tot) + "</td>";
                                HTML += "<td style=\"text-align:right;\">&nbsp;</td>";
                                HTML += "</tr>";

                                STP_PremiumAmt += decimal.Parse(DT.Rows[i]["trn_PreminumAmt"].ToString());
                                STP_PremPaidAmt += decimal.Parse(DT.Rows[i]["trn_paymentAmt"].ToString());
                                //STP_BaseComm += decimal.Parse(DT.Rows[i]["BaseCommission"].ToString());
                                STP_ORCcomm += decimal.Parse(DT.Rows[i]["ORCCommission"].ToString());
                                STP_TopUp += decimal.Parse(DT.Rows[i]["TopUpCommission"].ToString());
                                STP_Total += tot;

                                GT_PremiumAmt += decimal.Parse(DT.Rows[i]["trn_PreminumAmt"].ToString());
                                GT_PremPaidAmt += decimal.Parse(DT.Rows[i]["trn_paymentAmt"].ToString());
                                //GT_BaseComm += decimal.Parse(DT.Rows[i]["BaseCommission"].ToString());
                                GT_ORCcomm += decimal.Parse(DT.Rows[i]["ORCCommission"].ToString());
                                GT_TopUp += decimal.Parse(DT.Rows[i]["TopUpCommission"].ToString());
                                GT_Total += tot;

                                STC_PremiumAmt += decimal.Parse(DT.Rows[i]["trn_PreminumAmt"].ToString());
                                STC_PremPaidAmt += decimal.Parse(DT.Rows[i]["trn_paymentAmt"].ToString());
                                //STC_BaseComm += decimal.Parse(DT.Rows[i]["BaseCommission"].ToString());
                                STC_ORCcomm += decimal.Parse(DT.Rows[i]["ORCCommission"].ToString());
                                STC_TopUp += decimal.Parse(DT.Rows[i]["TopUpCommission"].ToString());
                                STC_Total += tot;
                            }
                        }
                        else
                        {
                            //product change so print the total of all the cunstomers data here first
                            //Sub-Total for Product

                            HTML += "<tr>";
                            HTML += "<td style=\"text-align:left; padding-left:4px\" colspan=\"6\">Product Total</td>";

                            HTML += "<td style=\"text-align:right;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(STP_PremiumAmt) + "</td>";
                            HTML += "<td style=\"text-align:right;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(STP_PremPaidAmt) + "</td>";
                            HTML += "<td style=\"text-align:right;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(STP_BaseComm) + "</td>";
                            HTML += "<td style=\"text-align:right;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(STP_ORCcomm) + "</td>";
                            HTML += "<td style=\"text-align:right;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(STP_Trail) + "</td>";
                            HTML += "<td style=\"text-align:right;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(STP_TopUp) + "</td>";

                            HTML += "<td style=\"text-align:right;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(STP_Total) + "</td>";
                            HTML += "<td style=\"text-align:right;\">&nbsp;</td>";
                            HTML += "</tr>";

                            STP_PremiumAmt = 0;
                            STP_PremPaidAmt = 0;
                            STP_BaseComm = 0;
                            STP_ORCcomm = 0;
                            STP_TopUp = 0;
                            STP_Total = 0;
                            //---end
                            //Now print COMPANY Total now
                            HTML += "<tr>";
                            HTML += "<td style=\"text-align:left; padding-left:4px\" colspan=\"6\">Company Total</td>";

                            HTML += "<td style=\"text-align:right;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(decimal.Parse(STC_PremiumAmt.ToString())) + "</td>";
                            HTML += "<td style=\"text-align:right;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(decimal.Parse(STC_PremPaidAmt.ToString())) + "</td>";
                            HTML += "<td style=\"text-align:right;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(decimal.Parse(STC_BaseComm.ToString())) + "</td>";
                            HTML += "<td style=\"text-align:right;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(decimal.Parse(STC_ORCcomm.ToString())) + "</td>";
                            HTML += "<td style=\"text-align:right;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(STC_Trail) + "</td>";
                            HTML += "<td style=\"text-align:right;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(STC_TopUp) + "</td>";

                            HTML += "<td style=\"text-align:right;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(decimal.Parse(STC_Total.ToString())) + "</td>";
                            HTML += "<td style=\"text-align:right;\">&nbsp;</td>";
                            HTML += "</tr>";

                            STC_PremiumAmt = 0;
                            STC_PremPaidAmt = 0;
                            STC_BaseComm = 0;
                            STC_ORCcomm = 0;
                            STC_TopUp = 0;
                            STC_Total = 0;
                            //---end
                            Company = DT.Rows[i]["companyID"].ToString();
                            Product = DT.Rows[i]["trn_Scheme"].ToString();
                            clients = DT.Rows[i]["trn_ContactID"].ToString();
                            HTML += "<tr><td colspan=\"14\" style=\"text-align:left; padding-left:1px\">" + DT.Rows[i]["company"].ToString() + "</td></tr>";
                            HTML += "<tr><td colspan=\"14\" style=\"text-align:left; padding-left:2px\">" + DT.Rows[i]["Product"].ToString() + "</td></tr>";
                            HTML += "<tr>";
                            HTML += "<td style=\"text-align:left; padding-left:3px\">" + DT.Rows[i]["client"].ToString() + "</td>";
                            HTML += "<td style=\"text-align:left;\">" + DT.Rows[i]["trn_PolicyNo"].ToString() + "</td>";
                            HTML += "<td style=\"text-align:left;\">" + DT.Rows[i]["trn_AppNo"].ToString() + "</td>";
                            HTML += "<td style=\"text-align:left;\">" + DT.Rows[i]["BusCalss"].ToString() + "</td>";
                            if (DT.Rows[i]["trn_LoginDate"] != DBNull.Value)
                                HTML += "<td style=\"text-align:left;\">" + DT.Rows[i]["trn_LoginDate"].ToString() + "</td>";
                            else
                                HTML += "<td style=\"text-align:left;\">&nbsp;</td>";
                            HTML += "<td style=\"text-align:left;\">" + DT.Rows[i]["trn_IssueDate"].ToString() + "</td>";
                            HTML += "<td style=\"text-align:right;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(decimal.Parse(DT.Rows[i]["trn_PreminumAmt"].ToString())) + "</td>";
                            HTML += "<td style=\"text-align:right;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(decimal.Parse(DT.Rows[i]["trn_paymentAmt"].ToString())) + "</td>";
                            if (DT.Rows[i]["type"].ToString() == "B")
                            {
                                HTML += "<td style=\"text-align:right;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(decimal.Parse(DT.Rows[i]["BaseCommission"].ToString())) + "</td>";
                                HTML += "<td style=\"text-align:right;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(decimal.Parse(DT.Rows[i]["ORCCommission"].ToString())) + "</td>";
                                HTML += "<td style=\"text-align:right;\">&nbsp;</td>";
                                STP_BaseComm += decimal.Parse(DT.Rows[i]["BaseCommission"].ToString());
                                GT_BaseComm += decimal.Parse(DT.Rows[i]["BaseCommission"].ToString());
                                STC_BaseComm += decimal.Parse(DT.Rows[i]["BaseCommission"].ToString());
                            }
                            else
                            {
                                HTML += "<td style=\"text-align:right;\">&nbsp;</td>";
                                HTML += "<td style=\"text-align:right;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(decimal.Parse(DT.Rows[i]["ORCCommission"].ToString())) + "</td>";
                                HTML += "<td style=\"text-align:right;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(decimal.Parse(DT.Rows[i]["BaseCommission"].ToString())) + "</td>";
                                STP_Trail += decimal.Parse(DT.Rows[i]["BaseCommission"].ToString());
                                GT_Trail += decimal.Parse(DT.Rows[i]["BaseCommission"].ToString());
                                STC_Trail += decimal.Parse(DT.Rows[i]["BaseCommission"].ToString());
                            }
                            HTML += "<td style=\"text-align:right;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(decimal.Parse(DT.Rows[i]["TopUpCommission"].ToString())) + "</td>";

                            decimal tot = decimal.Parse(DT.Rows[i]["BaseCommission"].ToString()) + decimal.Parse(DT.Rows[i]["ORCCommission"].ToString()) + decimal.Parse(DT.Rows[i]["TopUpCommission"].ToString());
                            HTML += "<td style=\"text-align:right;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(decimal.Parse(tot.ToString())) + "</td>";
                            HTML += "<td style=\"text-align:right;\">&nbsp;</td>";
                            HTML += "</tr>";

                            STP_PremiumAmt += decimal.Parse(DT.Rows[i]["trn_PreminumAmt"].ToString());
                            STP_PremPaidAmt += decimal.Parse(DT.Rows[i]["trn_paymentAmt"].ToString());
                            //STP_BaseComm += decimal.Parse(DT.Rows[i]["BaseCommission"].ToString());
                            STP_ORCcomm += decimal.Parse(DT.Rows[i]["ORCCommission"].ToString());
                            STP_TopUp += decimal.Parse(DT.Rows[i]["TopUpCommission"].ToString());
                            STP_Total += tot;

                            GT_PremiumAmt += decimal.Parse(DT.Rows[i]["trn_PreminumAmt"].ToString());
                            GT_PremPaidAmt += decimal.Parse(DT.Rows[i]["trn_paymentAmt"].ToString());
                            //GT_BaseComm += decimal.Parse(DT.Rows[i]["BaseCommission"].ToString());
                            GT_ORCcomm += decimal.Parse(DT.Rows[i]["ORCCommission"].ToString());
                            GT_TopUp += decimal.Parse(DT.Rows[i]["TopUpCommission"].ToString());
                            GT_Total += tot;

                            STC_PremiumAmt = decimal.Parse(DT.Rows[i]["trn_PreminumAmt"].ToString());
                            STC_PremPaidAmt = decimal.Parse(DT.Rows[i]["trn_paymentAmt"].ToString());
                            //STC_BaseComm = decimal.Parse(DT.Rows[i]["BaseCommission"].ToString());
                            STC_ORCcomm = decimal.Parse(DT.Rows[i]["ORCCommission"].ToString());
                            STC_TopUp += decimal.Parse(DT.Rows[i]["TopUpCommission"].ToString());
                            STC_Total = tot;
                        }

                    }
                }
                //product change so print the total of all the cunstomers data here first
                //Sub-Total for Product

                HTML += "<tr>";
                HTML += "<td style=\"text-align:left; padding-left:4px\" colspan=\"6\">Product Total</td>";

                HTML += "<td style=\"text-align:right;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(decimal.Parse(STP_PremiumAmt.ToString())) + "</td>";
                HTML += "<td style=\"text-align:right;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(decimal.Parse(STP_PremPaidAmt.ToString())) + "</td>";
                HTML += "<td style=\"text-align:right;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(decimal.Parse(STP_BaseComm.ToString())) + "</td>";
                HTML += "<td style=\"text-align:right;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(decimal.Parse(STP_ORCcomm.ToString())) + "</td>";
                HTML += "<td style=\"text-align:right;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(decimal.Parse(STP_Trail.ToString())) + "</td>";
                HTML += "<td style=\"text-align:right;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(STP_TopUp) + "</td>";

                HTML += "<td style=\"text-align:right;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(decimal.Parse(STP_Total.ToString())) + "</td>";
                HTML += "<td style=\"text-align:right;\">&nbsp;</td>";
                HTML += "</tr>";

                STP_PremiumAmt = 0;
                STP_PremPaidAmt = 0;
                STP_BaseComm = 0;
                STP_ORCcomm = 0;
                STP_TopUp = 0;
                STP_Total = 0;
                //---end
                //Now print COMPANY Total now
                HTML += "<tr>";
                HTML += "<td style=\"text-align:left; padding-left:4px\" colspan=\"6\">Company Total</td>";

                HTML += "<td style=\"text-align:right;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(decimal.Parse(STC_PremiumAmt.ToString())) + "</td>";
                HTML += "<td style=\"text-align:right;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(decimal.Parse(STC_PremPaidAmt.ToString())) + "</td>";
                HTML += "<td style=\"text-align:right;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(decimal.Parse(STC_BaseComm.ToString())) + "</td>";
                HTML += "<td style=\"text-align:right;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(decimal.Parse(STC_ORCcomm.ToString())) + "</td>";
                HTML += "<td style=\"text-align:right;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(decimal.Parse(STC_Trail.ToString())) + "</td>";
                HTML += "<td style=\"text-align:right;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(STC_TopUp) + "</td>";

                HTML += "<td style=\"text-align:right;\">" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(decimal.Parse(STC_Total.ToString())) + "</td>";
                HTML += "<td style=\"text-align:right;\">&nbsp;</td>";
                HTML += "</tr>";

                STC_PremiumAmt = 0;
                STC_PremPaidAmt = 0;
                STC_BaseComm = 0;
                STC_ORCcomm = 0;
                STC_TopUp = 0;
                STC_Total = 0;
                //---end
                //Now print GRAND Total now GRAND TOTAL here
                HTML += "<tr>";
                HTML += "<td style=\"text-align:left; padding-left:5px\" colspan=\"6\"><strong>Grand Total</strong></td>";
                //HTML += "<td style=\"text-align:left;\"></td>";
                HTML += "<td style=\"text-align:right;\"><strong>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(decimal.Parse(GT_PremiumAmt.ToString())) + "</strong></td>";
                HTML += "<td style=\"text-align:right;\"><strong>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(decimal.Parse(GT_PremPaidAmt.ToString())) + "</strong></td>";
                HTML += "<td style=\"text-align:right;\"><strong>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(decimal.Parse(GT_BaseComm.ToString())) + "</strong></td>";
                HTML += "<td style=\"text-align:right;\"><strong>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(decimal.Parse(GT_ORCcomm.ToString())) + "</strong></td>";
                HTML += "<td style=\"text-align:right;\"><strong>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(GT_Trail) + "</strong></td>";
                HTML += "<td style=\"text-align:right;\"><strong>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(GT_TopUp) + "</strong></td>";

                HTML += "<td style=\"text-align:right;\"><strong>" + oconverter.getFormattedvaluecheckorginaldecimaltwoorfour(decimal.Parse(GT_Total.ToString())) + "</strong></td>";
                HTML += "<td style=\"text-align:right;\">&nbsp;</td>";
                HTML += "</tr>";
                //---end
            }
            else
                HTML += "<tr><td colspan=\"14\" style=\"text-align:center\">Data Not Found!</td></tr>";
            HTML += "</table>";
            printReady.InnerHtml = HTML;
        }
    }
}