using System;
using System.Data;
using System.Web;
using BusinessLogicLayer;

namespace ERP.OMS.Reports
{
    public partial class Reports_frmReport_CommissionReceivableReport_internal : System.Web.UI.Page
    {
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        BusinessLogicLayer.Converter Oconverter = new BusinessLogicLayer.Converter();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                hdFdate.Value = Request.QueryString["Fdate"].ToString();
                hdTdate.Value = Request.QueryString["Tdate"].ToString();
                hdCompany.Value = Request.QueryString["CompID"].ToString();
                hdType.Value = Request.QueryString["Type"].ToString();
                lblCompany.Text = Request.QueryString["CompName"].ToString();
                lblFromDate.Text = Request.QueryString["Fdate"].ToString().Split('/')[1] + "-" + Request.QueryString["Fdate"].ToString().Split('/')[0] + "-" + Request.QueryString["Fdate"].ToString().Split('/')[2];
                lblToDate.Text = Request.QueryString["Tdate"].ToString().Split('/')[1] + "-" + Request.QueryString["Tdate"].ToString().Split('/')[0] + "-" + Request.QueryString["Tdate"].ToString().Split('/')[2];
                BindGrid();
                //Page.ClientScript.RegisterStartupScript(GetType(), "height", "<script>height();</script>");
            }
            else
                BindGrid();
        }
        private void BindGrid()
        {
            DataTable DT = new DataTable();
            if (Request.QueryString["Type"].ToString() == "Prod")
                DT = oDBEngine.GetDataTable(" (select * from (select *,(select dbo.Insurance_CommissionCalculation(trn_Scheme,trn_transMode,trn_BusinessClass,trn_paymentAmt,'BA',trn_PreminumAmt,'Base',issueDate,trn_PaymentDate )) as BaseCommission,(select dbo.Insurance_CommissionCalculation(trn_Scheme,trn_transMode,trn_BusinessClass,trn_paymentAmt,'OR',trn_PreminumAmt,'Base',issueDate,trn_PaymentDate )) as ORCCommission,(select dbo.Insurance_CommissionCalculation(trn_Scheme,trn_transMode,trn_BusinessClass,trn_paymentAmt,'TopUp',trn_PreminumAmt,'Base',issueDate,trn_PaymentDate  )) as TopUpCommission ,0 as TrailCommission " +
                        " from ( " +
                        " (select trn_id,(select prds_description from tbl_master_products where prds_internalID=trn_scheme) as company " +
                        " ,(select prd_insurerName from tbl_master_productsDetails where prd_internalID=trn_scheme ) companyID,trn_TransNo,trn_AppNo,trn_PolicyNo,trn_Scheme,trn_transMode,trn_BusinessClass,trn_ContactID,(case trn_BusinessClass when 'I' then 'Individual-Resident' when 'C' then 'Corporate' when 'T' then 'Trust' when 'N' then 'Individual-Non Resident' when 'A' then 'Any' else 'N/A' end) as BusCalss,convert(varchar,trn_LoginDate,5) as trn_LoginDate,convert(varchar,trn_IssueDate,5) as trn_IssueDate,trn_IssueDate as issueDate,'B' as type" +
                        " from tbl_trans_insmain ,tbl_master_user,tbl_master_contact where trn_id not in (select A.trn_id from tbl_trans_insmain A where dateadd(mm,12,dateadd(dd,trn_FreeLookupPeriod,trn_IssueDate)) <= '" + Request.QueryString["Fdate"].ToString() + "' ) " +
                        " and trn_status=4 and tbl_trans_insmain.CreateUser=tbl_master_user.user_id and tbl_master_user.user_contactID=tbl_master_contact.cnt_internalID and tbl_master_contact.cnt_branchid in (" + HttpContext.Current.Session["userbranchHierarchy"] + ")) as A inner join" +
                        " (select trn_id AS TT,trn_PreminumAmt,trn_paymentAmt,trn_transNo AS TTT,trn_PaymentDate from tbl_trans_insdetail " +
                        " where  cast(trn_PaymentDate as datetime) between cast('" + Request.QueryString["Fdate"].ToString() + "' as datetime) and cast('" + Request.QueryString["Tdate"].ToString() + "' as datetime) and trn_PaymentAmt is not NULL" +
                        " ) as B on A.trn_TransNo=B.TTT) where companyID='" + Request.QueryString["CompID"].ToString() + "'" +
                        " ) as D " +
                        " Union (select *,'0' as BaseCommission,'0' as ORCCommission ,(select dbo.Insurance_CommissionCalculation(trn_Scheme,trn_transMode,trn_BusinessClass,trn_paymentAmt,'TopUp',trn_PreminumAmt,'Trail',issueDate,trn_PaymentDate  )) as TopUpCommission,(select dbo.Insurance_CommissionCalculation(trn_Scheme,trn_transMode,trn_BusinessClass,trn_paymentAmt,'BA',trn_PreminumAmt,'Trail',issueDate,trn_PaymentDate )) as TrailCommission " +
                        " from ((select trn_id,(select prds_description from tbl_master_products where prds_internalID=trn_scheme) as company " +
                        " ,(select prd_insurerName from tbl_master_productsDetails where prd_internalID=trn_scheme ) companyID,trn_TransNo,trn_AppNo,trn_PolicyNo,trn_Scheme,trn_transMode,trn_BusinessClass,trn_ContactID,(case trn_BusinessClass when 'I' then 'Individual-Resident' when 'C' then 'Corporate' when 'T' then 'Trust' when 'N' then 'Individual-Non Resident' when 'A' then 'Any' else 'N/A' end) as BusCalss,convert(varchar,trn_LoginDate,5) as trn_LoginDate,convert(varchar,trn_IssueDate,5) as trn_IssueDate,trn_IssueDate as issueDate,'T' as type" +
                        " from tbl_trans_insmain ,tbl_master_user,tbl_master_contact" +
                        " where trn_id  in (select A.trn_id from tbl_trans_insmain A where dateadd(mm,12,dateadd(dd,trn_FreeLookupPeriod,trn_IssueDate)) < '" + Request.QueryString["Fdate"].ToString() + "' ) " +
                        " and trn_status=4  and tbl_trans_insmain.CreateUser=tbl_master_user.user_id and tbl_master_user.user_contactID=tbl_master_contact.cnt_internalID and tbl_master_contact.cnt_branchid in (" + HttpContext.Current.Session["userbranchHierarchy"] + ")" +
                        " ) as A inner join (select trn_id AS TT,trn_PreminumAmt,trn_paymentAmt,trn_transNo AS TTT,trn_PaymentDate from tbl_trans_insdetail " +
                        " where  cast(trn_PaymentDate as datetime) between cast('" + Request.QueryString["Fdate"].ToString() + "' as datetime) and cast('" + Request.QueryString["Tdate"].ToString() + "' as datetime) and trn_PaymentAmt is not NULL" +
                        " ) as B on A.trn_TransNo=B.TTT) )) as D ",
                        " D.company,sum(D.BaseCommission) as BaseCommission,sum(D.ORCCommission) as ORCCommission,D.trn_Scheme as ID,sum(D.trn_paymentAmt) as PayedAmt,'' as RecCommission ,sum(TopUpCommission) as TopUpCommission,sum(TrailCommission) as TrailCommission ",
                        null, " D.company ,D.trn_Scheme ", " D.company ");
            else
                DT = oDBEngine.GetDataTable(" (select * from (select *,(select dbo.Insurance_CommissionCalculation(trn_Scheme,trn_transMode,trn_BusinessClass,trn_paymentAmt,'BA',trn_PreminumAmt,'Base',issueDate,trn_PaymentDate )) as BaseCommission,(select dbo.Insurance_CommissionCalculation(trn_Scheme,trn_transMode,trn_BusinessClass,trn_paymentAmt,'OR',trn_PreminumAmt,'Base',issueDate,trn_PaymentDate )) as ORCCommission,(select dbo.Insurance_CommissionCalculation(trn_Scheme,trn_transMode,trn_BusinessClass,trn_paymentAmt,'TopUp',trn_PreminumAmt,'Base',issueDate,trn_PaymentDate  )) as TopUpCommission ,0 as TrailCommission " +
                        " from ( " +
                        " (select trn_id,(select isnull(RTRIM(cnt_firstName),'')+' '+isnull(RTRIM(cnt_middleName),'')+' '+isnull(RTRIM(cnt_lastName),'')+' ['+isnull(RTRIM(cnt_UCC),'')+']' from tbl_master_contact where cnt_internalid=trn_ContactID) as company " +
                        " ,(select prd_insurerName from tbl_master_productsDetails where prd_internalID=trn_scheme ) companyID,trn_TransNo,trn_AppNo,trn_PolicyNo,trn_Scheme,trn_transMode,trn_BusinessClass,trn_ContactID,(case trn_BusinessClass when 'I' then 'Individual-Resident' when 'C' then 'Corporate' when 'T' then 'Trust' when 'N' then 'Individual-Non Resident' when 'A' then 'Any' else 'N/A' end) as BusCalss,convert(varchar,trn_LoginDate,5) as trn_LoginDate,convert(varchar,trn_IssueDate,5) as trn_IssueDate,trn_IssueDate as issueDate,'B' as type" +
                        " from tbl_trans_insmain ,tbl_master_user,tbl_master_contact where trn_id not in (select A.trn_id from tbl_trans_insmain A where dateadd(mm,12,dateadd(dd,trn_FreeLookupPeriod,trn_IssueDate)) <= '" + Request.QueryString["Fdate"].ToString() + "' ) " +
                        " and trn_status=4 and tbl_trans_insmain.CreateUser=tbl_master_user.user_id and tbl_master_user.user_contactID=tbl_master_contact.cnt_internalID and tbl_master_contact.cnt_branchid in (" + HttpContext.Current.Session["userbranchHierarchy"] + ")) as A inner join" +
                        " (select trn_id AS TT,trn_PreminumAmt,trn_paymentAmt,trn_transNo AS TTT,trn_PaymentDate from tbl_trans_insdetail " +
                        " where  cast(trn_PaymentDate as datetime) between cast('" + Request.QueryString["Fdate"].ToString() + "' as datetime) and cast('" + Request.QueryString["Tdate"].ToString() + "' as datetime) and trn_PaymentAmt is not NULL" +
                        " ) as B on A.trn_TransNo=B.TTT) where companyID='" + Request.QueryString["CompID"].ToString() + "' and trn_Scheme='" + Request.QueryString["ProdID"].ToString() + "'" +
                        " ) as D " +
                        " Union (select *,'0' as BaseCommission,'0' as ORCCommission ,(select dbo.Insurance_CommissionCalculation(trn_Scheme,trn_transMode,trn_BusinessClass,trn_paymentAmt,'TopUp',trn_PreminumAmt,'Trail',issueDate,trn_PaymentDate  )) as TopUpCommission,(select dbo.Insurance_CommissionCalculation(trn_Scheme,trn_transMode,trn_BusinessClass,trn_paymentAmt,'BA',trn_PreminumAmt,'Trail',issueDate,trn_PaymentDate )) as TrailCommission " +
                        " from ((select trn_id,(select isnull(RTRIM(cnt_firstName),'')+' '+isnull(RTRIM(cnt_middleName),'')+' '+isnull(RTRIM(cnt_lastName),'')+' ['+isnull(RTRIM(cnt_UCC),'')+']' from tbl_master_contact where cnt_internalid=trn_ContactID) as company " +
                        " ,(select prd_insurerName from tbl_master_productsDetails where prd_internalID=trn_scheme ) companyID,trn_TransNo,trn_AppNo,trn_PolicyNo,trn_Scheme,trn_transMode,trn_BusinessClass,trn_ContactID,(case trn_BusinessClass when 'I' then 'Individual-Resident' when 'C' then 'Corporate' when 'T' then 'Trust' when 'N' then 'Individual-Non Resident' when 'A' then 'Any' else 'N/A' end) as BusCalss,convert(varchar,trn_LoginDate,5) as trn_LoginDate,convert(varchar,trn_IssueDate,5) as trn_IssueDate,trn_IssueDate as issueDate,'T' as type" +
                        " from tbl_trans_insmain ,tbl_master_user,tbl_master_contact" +
                        " where trn_id  in (select A.trn_id from tbl_trans_insmain A where dateadd(mm,12,dateadd(dd,trn_FreeLookupPeriod,trn_IssueDate)) < '" + Request.QueryString["Fdate"].ToString() + "' ) " +
                        " and trn_status=4  and tbl_trans_insmain.CreateUser=tbl_master_user.user_id and tbl_master_user.user_contactID=tbl_master_contact.cnt_internalID and tbl_master_contact.cnt_branchid in (" + HttpContext.Current.Session["userbranchHierarchy"] + ")" +
                        " ) as A inner join (select trn_id AS TT,trn_PreminumAmt,trn_paymentAmt,trn_transNo AS TTT,trn_PaymentDate from tbl_trans_insdetail " +
                        " where  cast(trn_PaymentDate as datetime) between cast('" + Request.QueryString["Fdate"].ToString() + "' as datetime) and cast('" + Request.QueryString["Tdate"].ToString() + "' as datetime) and trn_PaymentAmt is not NULL" +
                        " ) as B on A.trn_TransNo=B.TTT) where companyID='" + Request.QueryString["CompID"].ToString() + "' and trn_Scheme='" + Request.QueryString["ProdID"].ToString() + "' )) as D ",
                        " D.company,sum(D.BaseCommission) as BaseCommission,sum(D.ORCCommission) as ORCCommission,D.trn_ContactID as ID,sum(D.trn_paymentAmt) as PayedAmt,'' as RecCommission  ,sum(TopUpCommission) as TopUpCommission,sum(TrailCommission) as TrailCommission ",
                        null, "  D.company,D.trn_ContactID ", " D.company ");
            grdInsuranceCommission.DataSource = DT;
            grdInsuranceCommission.DataBind();
            if (Request.QueryString["Type"].ToString() != "Prod")
            {
                grdInsuranceCommission.Columns[7].Visible = false;
            }
            //Page.ClientScript.RegisterStartupScript(GetType(), "height", "<script>height();</script>");
        }
        protected void grdInsuranceCommission_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            BindGrid();
        }

    }
}