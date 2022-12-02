using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using CrystalDecisions.CrystalReports.Engine;
using BusinessLogicLayer;
namespace ERP.OMS.Reports
{
    public partial class frmReport_SalrySlip : System.Web.UI.Page
    {

        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
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
        }
        protected void btnSalarySlip_Click(object sender, EventArgs e)
        {

            DataSet ds = new DataSet();

            using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                //cmd.CommandText = "select  rde_id,(select sal_name from  tbl_master_salutation where sal_id=rde_Salutation) as Salutation,rde_Name ,rde_ResidenceLocation,(select cmp_name from  tbl_master_Company where cmp_id=rde_Company)as company,(select branch_description from tbl_master_branch where branch_id= rde_Branch)as Branch,(select deg_designation from tbl_master_Designation where deg_id=rde_Designation) as Designation,rde_ApprovedCTC  from tbl_trans_RecruitmentDetailTemp where rde_id in (" + data + ") and rde_status='Y'";
                cmd.CommandText = "select mc.cnt_UCC, mc.cnt_firstname+' ' + mc.cnt_middlename+' '+ cnt_lastName as Staffname,(select mcom .Com_Add from tbl_trans_employeectc Ectc inner join tbl_master_company mcom on Ectc.emp_Organization=mcom.cmp_id and Ectc.emp_cntId=mc.cnt_internalId ) as OrganisationAdd,(select mb.bnk_bankname from tbl_trans_contactBankDetails BD inner join tbl_master_Bank MB on MB.bnk_id=BD.cbd_bankCode where BD.cbd_cntId=mc.cnt_internalId) As bankname,(select mcom.Com_logopath from tbl_trans_employeectc Ectc inner join tbl_master_company mcom on Ectc.emp_Organization=mcom.cmp_id and Ectc.emp_cntId=mc.cnt_internalId ) as logopath,(select mb.branch_description from tbl_master_branch mb where mb.branch_id=mc.cnt_branchid   ) as Branch,(select md.deg_designation from tbl_trans_employeectc Ectc inner join tbl_master_designation md on Ectc.emp_Designation=md.deg_id and Ectc.emp_cntId=mc.cnt_internalId ) as Designation,(select mcc.cost_description from tbl_trans_employeectc Ectc inner join tbl_master_costCenter mcc on Ectc.emp_Department=mcc.cost_id and Ectc.emp_cntId=mc.cnt_internalId ) as Department,ts.sal_paidday,TS.Sal_Basic,Ts.Sal_HRA ,TS.Sal_Convnc ,TS.Sal_MedlReim,TS.Sal_SplEarng,TS.Sal_OthrAllownce,TS.Sal_LeaveEncash,TS. Sal_LTA,TS.Sal_GrossPay,Ts.Sal_PF,TS.Sal_ProfTax,TS.Sal_TDS,TS.sal_ESI,TS.Sal_AdvncSal,TS.Sal_LWPDeduc,TS.Sal_OtherDeduc,Ts.Sal_NetAmtPay,month(TS.Sal_Date) as smonth,convert(varchar(20),year(TS.Sal_Date)) as syear from tbl_master_contact mc inner join tbl_trans_Salary TS on mc.cnt_internalId=TS.Sal_ContanctID where mc.cnt_internalId='" + HttpContext.Current.Session["usercontactID"] + "' and month(TS.Sal_Date)=" + txtDOB.Date.Month.ToString() + " and year(TS.Sal_Date)=" + txtDOB.Date.Year.ToString() + "";
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                cmd.CommandTimeout = 0;
                ds.Reset();
                da.Fill(ds);
                da.Dispose();
                if (ds.Tables[0].Rows.Count > 0)
                {
                    //  oDBEngine.SetFieldValue("tbl_trans_RecruitmentDetailTemp", "rde_GenerateDate='" + oDBEngine.GetDate() + "',rde_GenerateUser='" + HttpContext.Current.Session["userid"] + "'", "rde_Id in (select  rde_id from tbl_trans_RecruitmentDetailTemp WHERE rde_id in (" + data + ") and rde_status='Y')");
                    ReportDocument report = new ReportDocument();
                    // ds.Tables[0].WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\ContactDetails.xsd");
                    //  ds.Tables[0].WriteXmlSchema("C:\\XSDFile\\OfferLetter.xsd");
                    //   ds.Tables[0].WriteXmlSchema("d:\\commonfolderinfluxcrm\\reports\\OfferLetter.xsd");
                    // ds.Tables[0].WriteXmlSchema("\\Reports\\ContactDetails.xsd");
                    //ds.Tables[0].WriteXmlSchema("d:\\SalarySlip.xsd");
                    string tmpPdfPath = string.Empty;
                    tmpPdfPath = HttpContext.Current.Server.MapPath("..\\Reports\\salarySlip.rpt");
                    report.Load(tmpPdfPath);
                    report.SetDataSource(ds.Tables[0]);
                    report.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, HttpContext.Current.Response, true, "salarySlip");

                    report.Dispose();
                    GC.Collect();

                }
                else
                {
                    this.Page.ClientScript.RegisterStartupScript(GetType(), "script1", "<script>alert('Salary not available!.');</script>");

                }

            }
        }


        protected void btnReimbursment_Click(object sender, EventArgs e)
        {
            DataSet ds = new DataSet();

            using (SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                //cmd.CommandText = "select  rde_id,(select sal_name from  tbl_master_salutation where sal_id=rde_Salutation) as Salutation,rde_Name ,rde_ResidenceLocation,(select cmp_name from  tbl_master_Company where cmp_id=rde_Company)as company,(select branch_description from tbl_master_branch where branch_id= rde_Branch)as Branch,(select deg_designation from tbl_master_Designation where deg_id=rde_Designation) as Designation,rde_ApprovedCTC  from tbl_trans_RecruitmentDetailTemp where rde_id in (" + data + ") and rde_status='Y'";
                cmd.CommandText = "select mc.cnt_UCC, mc.cnt_firstname+' ' + mc.cnt_middlename+' '+ cnt_lastName as Staffname,(select mcom .Com_Add from tbl_trans_employeectc Ectc inner join tbl_master_company mcom on Ectc.emp_Organization=mcom.cmp_id and Ectc.emp_cntId=mc.cnt_internalId ) as OrganisationAdd,(select mb.bnk_bankname from tbl_trans_contactBankDetails BD inner join tbl_master_Bank MB on MB.bnk_id=BD.cbd_bankCode where BD.cbd_cntId=mc.cnt_internalId) As bankname,(select mcom.Com_logopath from tbl_trans_employeectc Ectc inner join tbl_master_company mcom on Ectc.emp_Organization=mcom.cmp_id and Ectc.emp_cntId=mc.cnt_internalId ) as logopath,(select mb.branch_description from tbl_master_branch mb where mb.branch_id=mc.cnt_branchid   ) as Branch,(select md.deg_designation from tbl_trans_employeectc Ectc inner join tbl_master_designation md on Ectc.emp_Designation=md.deg_id and Ectc.emp_cntId=mc.cnt_internalId ) as Designation,(select mcc.cost_description from tbl_trans_employeectc Ectc inner join tbl_master_costCenter mcc on Ectc.emp_Department=mcc.cost_id and Ectc.emp_cntId=mc.cnt_internalId ) as Department,ts.sal_paidday,TS.Sal_CarReimb,Ts.Sal_UniformAllownce ,TS.Sal_BooksSeminalAll ,TS.sal_Seminar,TS.sal_ReimbAdv,month(TS.Sal_Date) as smonth,convert(varchar(20),year(TS.Sal_Date)) as syear from tbl_master_contact mc inner join tbl_trans_Salary TS on mc.cnt_internalId=TS.Sal_ContanctID where mc.cnt_internalId='" + HttpContext.Current.Session["usercontactID"] + "' and month(TS.Sal_Date)=" + txtDOB.Date.Month.ToString() + " and year(TS.Sal_Date)=" + txtDOB.Date.Year.ToString() + "";
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                cmd.CommandTimeout = 0;
                ds.Reset();
                da.Fill(ds);
                da.Dispose();
                if (ds.Tables[0].Rows.Count > 0)
                {
                    //  oDBEngine.SetFieldValue("tbl_trans_RecruitmentDetailTemp", "rde_GenerateDate='" + oDBEngine.GetDate() + "',rde_GenerateUser='" + HttpContext.Current.Session["userid"] + "'", "rde_Id in (select  rde_id from tbl_trans_RecruitmentDetailTemp WHERE rde_id in (" + data + ") and rde_status='Y')");
                    ReportDocument report = new ReportDocument();
                    // ds.Tables[0].WriteXmlSchema(ConfigurationManager.AppSettings["SaveCSVsql"] + "\\Reports\\ContactDetails.xsd");
                    //  ds.Tables[0].WriteXmlSchema("C:\\XSDFile\\OfferLetter.xsd");
                    //   ds.Tables[0].WriteXmlSchema("d:\\commonfolderinfluxcrm\\reports\\OfferLetter.xsd");
                    // ds.Tables[0].WriteXmlSchema("\\Reports\\ContactDetails.xsd");
                    //ds.Tables[0].WriteXmlSchema("d:\\Reimburstment.xsd");
                    string tmpPdfPath = string.Empty;
                    tmpPdfPath = HttpContext.Current.Server.MapPath("..\\Reports\\ReimbursmentSlip.rpt");
                    report.Load(tmpPdfPath);
                    report.SetDataSource(ds.Tables[0]);
                    report.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, HttpContext.Current.Response, true, "ReimbursmentSlip");

                    report.Dispose();
                    GC.Collect();

                }
                else
                {
                    this.Page.ClientScript.RegisterStartupScript(GetType(), "script1", "<script>alert('Salary not available!');</script>");

                }

            }
        }
    }
}