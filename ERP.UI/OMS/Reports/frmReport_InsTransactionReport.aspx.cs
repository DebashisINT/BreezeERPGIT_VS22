using System;
using System.Data;
using System.Web;
using System.Web.UI;
using BusinessLogicLayer;

namespace ERP.OMS.Reports
{
    public partial class Reports_frmReport_InsTransactionReport : System.Web.UI.Page
    {
        static string data = string.Empty;
        public string pageAccess = "";
        BusinessLogicLayer.Converter OConvert = new BusinessLogicLayer.Converter();
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);

        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string sPath = HttpContext.Current.Request.Url.ToString();
                oDBEngine.Call_CheckPageaccessebility(sPath);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            txtInsurerCompany.Attributes.Add("onkeyup", "InsurerCompany(this,'getCompanyByLetters',event,'Insurance-Life')");
            txtFromDate.EditFormatString = OConvert.GetDateFormat("Date");
            txtToDate.EditFormatString = OConvert.GetDateFormat("Date");
            if (HttpContext.Current.Session["userid"] == null)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }
            //this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>height();</script>");
            GridBind();

        }


        protected void GridMessage_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            if (e.Parameters == "s")
            {
                GridMessage.Settings.ShowFilterRow = true;
            }
            else if (e.Parameters == "All")
            {
                GridMessage.FilterExpression = string.Empty;
            }


        }




        public void GridBind()
        {

            string startdate = txtFromDate.Date.Year.ToString() + "-" + txtFromDate.Date.Month.ToString() + "-" + txtFromDate.Date.Day.ToString();
            string Enddate = txtToDate.Date.Year.ToString() + "-" + txtToDate.Date.Month.ToString() + "-" + txtToDate.Date.Day.ToString();


            DataTable DT = new DataTable();
            if (cmbType.SelectedItem.Value.ToString() == "M")
            {
                DT = oDBEngine.GetDataTable("tbl_trans_insmainUPDATE as A   INNER JOIN tbl_master_contact ON A.trn_ContactId = tbl_master_contact.cnt_internalId INNER JOIN    tbl_master_branch as B ON A.trn_Branch = B.branch_Id INNER JOIN    tbl_master_products ON A.trn_Scheme = tbl_master_products.prds_internalId  ", "A.trn_IdMain AS TranID,A.trn_TransNo AS Id, CONVERT(VARCHAR(10), A.trn_RecDate, 105) AS RecieveDate,ISNULL(tbl_master_contact.cnt_firstName, ' ') + ' ' + ISNULL(tbl_master_contact.cnt_middleName, '') + ' '  + ISNULL(tbl_master_contact.cnt_lastName, '') AS Name, B.branch_description AS Branch, cast(A.trn_TransDate as datetime) AS TransDate, A.trn_sumAssured AS InsuredAmt,   A.trn_PremiumAmt AS PremiumAmount, A.trn_AppNo AS ApplicationNo, A.trn_PolicyNo AS PloicyNo,   tbl_master_products.prds_description AS PlanName,A.PlanName as [Plan],  CONVERT(VARCHAR(10), A.trn_IssueDate, 105) AS IssueDate ,  (select insu_nameOfCompany from tbl_master_insurerName where insu_internalId=A.[trn_SchemeCompany]) as InsCompany ,CASE isnull(A.trn_Status, 0)  WHEN 0 THEN 'Business in Hand' WHEN 1 THEN 'Cancelled' WHEN 2 THEN 'Canclled from inception' WHEN 3 THEN 'Cheque Bounce' WHEN 4 THEN 'Issue' WHEN 5 THEN   'Lapsed' WHEN 6 THEN 'Login' END AS Status,   CASE A.trn_transMode WHEN 0 THEN 'Monthly' WHEN 1 THEN 'Quaterly' WHEN 2 THEN 'Half Yearly' WHEN 3 THEN 'Yearly' ELSE 'Annualy'   END AS Mode, A.Status  as Stat,(select cnt_firstName +' '+cnt_middleName+' '+cnt_lastName  from tbl_master_contact where  cnt_internalid=A.trn_Referal)as Associate,(select cnt_firstName +' '+cnt_middleName+' '+cnt_lastName  from tbl_master_contact where  cnt_internalid=A.trn_Telecaller)as Telecaller", "RTRIM(A.trn_Branch) IN (" + HttpContext.Current.Session["userbranchHierarchy"].ToString() + ") And  A.trn_company = '" + Session["LastCompany"].ToString() + "' and A.trn_SchemeCompany='" + txtInsurerCompany_hidden.Value + "' and A.trn_RecDate >= '" + startdate + "' and A.trn_RecDate <= '" + Enddate + "'", " A.trn_IssueDate");
                GridMessage.Columns[3].Visible = true;
                GridMessage.Columns[10].Visible = false;
                GridMessage.Columns[11].Visible = false;
                GridMessage.Columns[8].Visible = true;
                GridMessage.Columns[9].Visible = true;

                GridMessage.Columns[12].Visible = true;
                //GridMessage.Columns[13].Visible = true;
            }
            else if (cmbType.SelectedItem.Value.ToString() == "U")
            {
                DT = oDBEngine.GetDataTable("tbl_trans_insmainUPDATE", "[trn_IdMain] as TranID,[trn_Company],(select insu_nameOfCompany from tbl_master_insurerName where insu_internalId=[trn_SchemeCompany]) as InsCompany,[trn_AppNo] as ApplicationNo, CONVERT(VARCHAR(10), trn_RecDate, 105) AS RecieveDate ,CONVERT(VARCHAR(10), trn_IssueDate, 105) as IssueDate,[trn_PolicyNo] as PloicyNo,[trn_PremiumAmt] as PremiumAmount,(select branch_description from tbl_master_branch where branch_id=trn_branch) as Branch,[trn_AnnualPremium],[trn_PolicyTerm],[CreateDate],	[CreateUser],[Region],[Location],[SalesUnitCode],	[CLNTNUM],	[CLNTNAME] as Name,	[CSCCODE],	[SMName],	[Advisor],	[AdvisorName],	[Status] as Stat,[Frequency],	[PlanName],	[Nature],	[PrmType],	[WRP],	[Month],	[Gross],(select cnt_firstName +' '+cnt_middleName+' '+cnt_lastName  from tbl_master_contact where  cnt_internalid=trn_Referal)as Associate,(select cnt_firstName +' '+cnt_middleName+' '+cnt_lastName  from tbl_master_contact where  cnt_internalid=trn_Telecaller)as Telecaller", "trn_Id is null and trn_company='" + Session["LastCompany"].ToString() + "' and trn_SchemeCompany='" + txtInsurerCompany_hidden.Value + "' and (CAST([trn_RecDate] AS datetime) >= CONVERT(varchar,'" + startdate + "', 101)) and (CAST([trn_RecDate] AS datetime) <= CONVERT(varchar,'" + Enddate + "', 101))");

                GridMessage.Columns[3].Visible = false;
                GridMessage.Columns[10].Visible = true;
                GridMessage.Columns[11].Visible = true;
                GridMessage.Columns[8].Visible = true;
                GridMessage.Columns[9].Visible = true;
                GridMessage.Columns[12].Visible = false;

            }
            else if (cmbType.SelectedItem.Value.ToString() == "N")
            {
                DT = oDBEngine.GetDataTable("tbl_trans_insmain as A   INNER JOIN tbl_master_contact ON A.trn_ContactId = tbl_master_contact.cnt_internalId INNER JOIN    tbl_master_branch as B ON A.trn_Branch = B.branch_Id INNER JOIN    tbl_master_products ON A.trn_Scheme = tbl_master_products.prds_internalId  ", "A.trn_TransNo AS TranID, CONVERT(VARCHAR(10), A.trn_RecDate, 105) AS RecieveDate,ISNULL(tbl_master_contact.cnt_firstName, ' ') + ' ' + ISNULL(tbl_master_contact.cnt_middleName, '') + ' '  + ISNULL(tbl_master_contact.cnt_lastName, '') AS Name, B.branch_description AS Branch, cast(A.trn_TransDate as datetime) AS TransDate, A.trn_sumAssured AS InsuredAmt,   A.trn_PremiumAmt AS PremiumAmount, A.trn_AppNo AS ApplicationNo, A.trn_PolicyNo AS PloicyNo,   tbl_master_products.prds_description AS PlanName, CONVERT(VARCHAR(10), A.trn_IssueDate, 105) AS IssueDate ,  (select insu_nameOfCompany from tbl_master_insurerName where insu_internalId=A.[trn_SchemeCompany]) as InsCompany ,CASE isnull(A.trn_Status, 0)  WHEN 0 THEN 'Business in Hand' WHEN 1 THEN 'Cancelled' WHEN 2 THEN 'Canclled from inception' WHEN 3 THEN 'Cheque Bounce' WHEN 4 THEN 'Issue' WHEN 5 THEN   'Lapsed' WHEN 6 THEN 'Login' END AS Stat,'' as Region,'' as Location,   CASE A.trn_transMode WHEN 0 THEN 'Monthly' WHEN 1 THEN 'Quaterly' WHEN 2 THEN 'Half Yearly' WHEN 3 THEN 'Yearly' ELSE 'Annualy'   END AS Mode,(select cnt_firstName +' '+cnt_middleName+' '+cnt_lastName  from tbl_master_contact where  cnt_internalid=A.trn_Referal)as Associate,(select cnt_firstName +' '+cnt_middleName+' '+cnt_lastName  from tbl_master_contact where  cnt_internalid=A.trn_Telecaller)as Telecaller", "RTRIM(A.trn_Branch) IN (" + HttpContext.Current.Session["userbranchHierarchy"].ToString() + ") And  A.trn_company = '" + Session["LastCompany"].ToString() + "' and A.trn_SchemeCompany='" + txtInsurerCompany_hidden.Value + "' and (CAST(A.trn_TransDate AS datetime) >= CONVERT(varchar,'" + startdate + "', 101)) and (CAST(A.trn_TransDate AS datetime) <= CONVERT(varchar,'" + Enddate + "', 101)) and A.trn_TransNo not in (select trn_TransNo from tbl_trans_insmainupdate where RTRIM(trn_Branch) IN (" + HttpContext.Current.Session["userbranchHierarchy"].ToString() + ") And  trn_company = '" + Session["LastCompany"].ToString() + "' and trn_SchemeCompany='" + txtInsurerCompany_hidden.Value + "' and (CAST(trn_RecDate AS datetime) >= CONVERT(varchar,'" + startdate + "', 101)) and (CAST(trn_RecDate AS datetime) <= CONVERT(varchar,'" + Enddate + "', 101)))", " A.trn_RecDate  ");
                GridMessage.Columns[8].Visible = false;
                GridMessage.Columns[9].Visible = false;
                GridMessage.Columns[10].Visible = false;
                GridMessage.Columns[11].Visible = false;
                GridMessage.Columns[12].Visible = true;


            }
            GridMessage.DataSource = DT;
            GridMessage.DataBind();
            this.Page.ClientScript.RegisterStartupScript(GetType(), "heighSCR", "<script>height();</script>");

        }

        protected void GridMessage_CustomJSProperties(object sender, DevExpress.Web.ASPxGridViewClientJSPropertiesEventArgs e)
        {
            e.Properties["cpHeight"] = "a";
        }

        protected void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 Filter = int.Parse(cmbExport.SelectedItem.Value.ToString());
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
            }
        }


        protected void BtnShow_Click(object sender, EventArgs e)
        {

        }
    }
}