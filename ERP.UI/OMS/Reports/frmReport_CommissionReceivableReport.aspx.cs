using System;
using System.Data;
using System.Web;
using System.Web.UI;
using BusinessLogicLayer;

namespace ERP.OMS.Reports
{
    public partial class Reports_frmReport_CommissionReceivableReport : System.Web.UI.Page
    {
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        BusinessLogicLayer.Converter Oconverter = new BusinessLogicLayer.Converter();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                dtFromDate.EditFormatString = Oconverter.GetDateFormat("Date");
                dtToDate.EditFormatString = Oconverter.GetDateFormat("Date");
                Page.ClientScript.RegisterStartupScript(GetType(), "height", "<script>height();</script>");
            }
            else
                BindGrid();
        }

        private void BindGrid()
        {
            DataTable DT = oDBEngine.GetDataTable(" (Select  * from  (select *,(select dbo.Insurance_CommissionCalculation1(trn_Scheme,trn_transMode,trn_BusinessClass,trn_paymentAmt,'BA',trn_PreminumAmt,'Base',issueDate,trn_PaymentDate )) as BaseCommission,(select dbo.Insurance_CommissionCalculation1(trn_Scheme,trn_transMode,trn_BusinessClass,trn_paymentAmt,'OR',trn_PreminumAmt,'Base',issueDate,trn_PaymentDate )) as ORCCommission,(select dbo.Insurance_CommissionCalculation1(trn_Scheme,trn_transMode,trn_BusinessClass,trn_paymentAmt,'TopUp',trn_PreminumAmt,'Base',issueDate,trn_PaymentDate  )) as TopUpCommission ,0 as TrailCommission from (" +
                 " (select trn_id,(select insu_nameOfCompany from tbl_master_insurerName where insu_internalId in(select prd_insurerName from tbl_master_productsDetails where prd_internalID=trn_scheme )) as company " +
                 " ,(select prd_insurerName from tbl_master_productsDetails where prd_internalID=trn_scheme ) companiID,trn_TransNo,trn_AppNo,trn_PolicyNo,trn_Scheme,trn_transMode,trn_BusinessClass,trn_ContactID,(case trn_BusinessClass when 'I' then 'Individual-Resident' when 'C' then 'Corporate' when 'T' then 'Trust' when 'N' then 'Individual-Non Resident' when 'A' then 'Any' else 'N/A' end) as BusCalss,convert(varchar,trn_LoginDate,5) as trn_LoginDate,convert(varchar,trn_IssueDate,5) as trn_IssueDate,trn_IssueDate as issueDate,'B' as type" +
                 " from tbl_trans_insmain ,tbl_master_user,tbl_master_contact where trn_id not in (select A.trn_id from tbl_trans_insmain A where dateadd(mm,12,dateadd(dd,trn_FreeLookupPeriod,trn_IssueDate)) <= '" + dtFromDate.Value + "' ) " +
                 " and trn_status=4 and tbl_trans_insmain.CreateUser=tbl_master_user.user_id and tbl_master_user.user_contactID=tbl_master_contact.cnt_internalID and tbl_master_contact.cnt_branchid in (" + HttpContext.Current.Session["userbranchHierarchy"] + ")" +
                 " ) as A inner join (select trn_id AS TT,trn_PreminumAmt,trn_paymentAmt,trn_transNo AS TTT,trn_PaymentDate from tbl_trans_insdetail " +
                 " where  cast(trn_PaymentDate as datetime) between cast('" + dtFromDate.Value + "' as datetime) and cast('" + dtToDate.Value + "' as datetime) and trn_PaymentAmt is not NULL) as B on A.trn_TransNo=B.TTT) ) as D " +
                 " Union (select *,'0' as BaseCommission,'0' as ORCCommission ,(select dbo.Insurance_CommissionCalculation1(trn_Scheme,trn_transMode,trn_BusinessClass,trn_paymentAmt,'TopUp',trn_PreminumAmt,'Trail',issueDate,trn_PaymentDate  )) as TopUpCommission,(select dbo.Insurance_CommissionCalculation1(trn_Scheme,trn_transMode,trn_BusinessClass,trn_paymentAmt,'BA',trn_PreminumAmt,'Trail',issueDate,trn_PaymentDate )) as TrailCommission " +
                 " from ((select trn_id,(select insu_nameOfCompany from tbl_master_insurerName where insu_internalId in(select prd_insurerName from tbl_master_productsDetails where prd_internalID=trn_scheme )) as company " +
                " ,(select prd_insurerName from tbl_master_productsDetails where prd_internalID=trn_scheme ) companyID,trn_TransNo,trn_AppNo,trn_PolicyNo,trn_Scheme,trn_transMode,trn_BusinessClass,trn_ContactID,(case trn_BusinessClass when 'I' then 'Individual-Resident' when 'C' then 'Corporate' when 'T' then 'Trust' when 'N' then 'Individual-Non Resident' when 'A' then 'Any' else 'N/A' end) as BusCalss,convert(varchar,trn_LoginDate,5) as trn_LoginDate,convert(varchar,trn_IssueDate,5) as trn_IssueDate,trn_IssueDate as issueDate,'T' as type" +
                " from tbl_trans_insmain ,tbl_master_user,tbl_master_contact" +
                " where trn_id  in (select A.trn_id from tbl_trans_insmain A where dateadd(mm,12,dateadd(dd,trn_FreeLookupPeriod,trn_IssueDate)) < '" + dtFromDate.Value + "' ) " +
                " and trn_status=4  and tbl_trans_insmain.CreateUser=tbl_master_user.user_id and tbl_master_user.user_contactID=tbl_master_contact.cnt_internalID and tbl_master_contact.cnt_branchid in (" + HttpContext.Current.Session["userbranchHierarchy"] + ")" +
                " ) as A inner join (select trn_id AS TT,trn_PreminumAmt,trn_paymentAmt,trn_transNo AS TTT,trn_PaymentDate from tbl_trans_insdetail " +
                " where  cast(trn_PaymentDate as datetime) between cast('" + dtFromDate.Value + "' as datetime) and cast('" + dtToDate.Value + "' as datetime) and trn_PaymentAmt is not NULL" +
                " ) as B on A.trn_TransNo=B.TTT) )) as D ",
                 " D.company,sum(D.BaseCommission) as BaseCommission,sum(D.ORCCommission) as ORCCommission,D.companiID,sum(D.trn_paymentAmt) as PayedAmt,'' as RecCommission ,sum(TopUpCommission) as TopUpCommission,sum(TrailCommission) as TrailCommission "
                 , null, " D.company ,D.companiID ", " D.company ");
            grdInsuranceCommission.DataSource = DT;
            grdInsuranceCommission.DataBind();
            Page.ClientScript.RegisterStartupScript(GetType(), "height", "<script>height();</script>");
        }
        protected void grdInsuranceCommission_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            BindGrid();
        }
        protected void grdInsuranceCommission_CustomJSProperties(object sender, DevExpress.Web.ASPxGridViewClientJSPropertiesEventArgs e)
        {
            e.Properties["cpHeight"] = 2;
        }
    }
}