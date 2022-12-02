using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogicLayer;

namespace ERP.OMS.Reports
{
    public partial class Reports_frmClientBankSearch : System.Web.UI.Page
    {
        BusinessLogicLayer.DBEngine odbengine = new BusinessLogicLayer.DBEngine(string.Empty);
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "script1", "<Script Language='JavaScript'>height();</script> ");



                Page.ClientScript.RegisterStartupScript(Page.GetType(), "script2", "<Script Language='JavaScript'>height();</script> ");


            }
        }
        protected void btnSearch_Click(object sender, System.EventArgs e)
        {
            if (chkDuplicate.Checked == false)
            {
                string AccountNumber = txtAccountNumber.Text.ToString();
                DataTable dt2 = new DataTable();
                if (AccountNumber != "")
                {


                    dt2 = odbengine.GetDataTable("tbl_trans_contactBankDetails,tbl_master_contact,tbl_master_bank", "cbd_id,isnull(cnt_firstname,'')+' '+isnull(cnt_middlename,'')+' '+isnull(cnt_lastName,'')+'['+ isnull(cnt_UCC,'')+']' as CustomerName,cbd_accountName as AccountName,cbd_accountType as AccountType,cbd_accountCategory as AccountCategory,cbd_bankcode,REPLACE(substring(LTRIM(RTRIM(cbd_accountNumber)), patindex('%[^0]%',cbd_accountNumber), 20),'-','') as AccountNumber,bnk_bankName as BankName,bnk_branchName as BranchName,bnk_micrno as MICR", "cbd_cntID=cnt_internalID and bnk_id=cbd_bankCode and REPLACE(substring(LTRIM(RTRIM(cbd_accountNumber)), patindex('%[^0]%',cbd_accountNumber), 20),'-','')=REPLACE(substring(LTRIM(RTRIM('" + AccountNumber + "')), patindex('%[^0]%','" + AccountNumber + "'), 20),'-','')");
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "script10", "<Script Language='JavaScript'>alert('Please Enter AccountNumber')</script> ");
                }
                if (dt2.Rows.Count > 0)
                {
                    GridView1.DataSource = dt2;
                    GridView1.DataBind();
                    txtAccountNumber.Text = "";
                }
                else
                {
                    GridView1.DataSource = dt2;
                    GridView1.DataBind();
                    txtAccountNumber.Text = "";

                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "script12", "<Script Language='JavaScript'>alert('No Records Found!')</script> ");
                }
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "script3", "<Script Language='JavaScript'>height();</script> ");
            }
            else if (chkDuplicate.Checked == true)
            {
                DataTable dt2 = new DataTable();
                dt2 = odbengine.GetDataTable("tbl_trans_contactBankDetails,tbl_master_contact,tbl_master_bank,(Select Distinct REPLACE(substring(LTRIM(RTRIM(cbd_accountNumber)), patindex('%[^0]%',cbd_accountNumber), 20),'-','')as AN ,cbd_bankCode as bkcode,COUNT(REPLACE(substring(LTRIM(RTRIM(cbd_accountNumber)), patindex('%[^0]%',cbd_accountNumber), 20),'-','')) as Occurance FROM tbl_trans_contactBankDetails GROUP BY REPLACE(substring(LTRIM(RTRIM(cbd_accountNumber)), patindex('%[^0]%',cbd_accountNumber), 20),'-',''),cbd_bankCode HAVING COUNT(REPLACE(substring(LTRIM(RTRIM(cbd_accountNumber)), patindex('%[^0]%',cbd_accountNumber), 20),'-',''))>1) as CC", "Distinct cbd_id,isnull(cnt_firstname,'')+' '+isnull(cnt_middlename,'')+' '+isnull(cnt_lastName,'')+'['+ isnull(cnt_UCC,'')+']' as CustomerName,cbd_accountName as AccountName,cbd_accountType as AccountType,cbd_accountCategory as AccountCategory,cbd_bankcode,REPLACE(substring(LTRIM(RTRIM(cbd_accountNumber)), patindex('%[^0]%',cbd_accountNumber), 20),'-','') as AccountNumber,bnk_bankName as BankName,bnk_branchName as BranchName,bnk_micrno as MICR ", "cbd_cntID=cnt_internalID and bnk_id=cbd_bankCode AND CC.bkcode=tbl_trans_contactBankDetails.cbd_bankCode AND CC.AN=REPLACE(substring(LTRIM(RTRIM(cbd_accountNumber)), patindex('%[^0]%',cbd_accountNumber), 20),'-','')");
                if (dt2.Rows.Count > 0)
                {
                    GridView1.DataSource = dt2;
                    GridView1.DataBind();
                    txtAccountNumber.Text = "";
                }
                else
                {
                    GridView1.DataSource = dt2;
                    GridView1.DataBind();
                    txtAccountNumber.Text = "";

                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "script18", "<Script Language='JavaScript'>alert('No Records Found!')</script> ");
                }

                Page.ClientScript.RegisterStartupScript(Page.GetType(), "script20", "<Script Language='JavaScript'>height();</script> ");
            }
        }
        protected void btnClose_Click(object sender, System.EventArgs e)
        {
            txtAccountNumber.Text = "";
            BindData();
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "script15", "<Script Language='JavaScript'>height();</script> ");
        }
        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            BindData();
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "script21", "<Script Language='JavaScript'>height();</script> ");
        }
        protected void BindData()
        {
            if (chkDuplicate.Checked == false)
            {
                DataTable dt1 = new DataTable();

                dt1 = odbengine.GetDataTable("tbl_trans_contactBankDetails,tbl_master_contact,tbl_master_bank", "cbd_id,isnull(cnt_firstname,'')+' '+isnull(cnt_middlename,'')+' '+isnull(cnt_lastName,'')+'['+ isnull(cnt_UCC,'')+']' as CustomerName,cbd_accountName as AccountName,cbd_accountType as AccountType,cbd_accountCategory as AccountCategory,REPLACE(substring(LTRIM(RTRIM(cbd_accountNumber)), patindex('%[^0]%',cbd_accountNumber), 20),'-','') as AccountNumber,bnk_bankName as BankName,bnk_branchName as BranchName,bnk_micrno as MICR", "cbd_cntID=cnt_internalID and bnk_id=cbd_bankCode", "CustomerName");

                if (dt1.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt1.Rows)
                    {
                        //lblCustomerName.Text = dr["CNAME"].ToString();
                        //lblUCC.Text = dr["UCC"].ToString();
                        //lblBannedOrderDate.Text = Convert.ToDateTime(dr["BannedOrderDate"]).ToString("dd-MMM-yyyy");
                        //lblParticulars.Text = dr["Particulars"].ToString();
                        //lblBanPeriod.Text = dr["BanPeriod"].ToString();
                        //lblCircularLink.Text = dr["BannedEntity_CircularLink"].ToString();
                        //Session["BannedEntityCircularLink"] = dr["CircularLink"].ToString();

                    }
                    GridView1.DataSource = dt1;
                    GridView1.DataBind();
                }
                else
                {
                    Label3.Text = "No Record Found....";
                    //Response.Write("No Record Found....");


                }


            }
            else if (chkDuplicate.Checked == true)
            {
                DataTable dt2 = new DataTable();
                dt2 = odbengine.GetDataTable("tbl_trans_contactBankDetails,tbl_master_contact,tbl_master_bank,(Select Distinct REPLACE(substring(LTRIM(RTRIM(cbd_accountNumber)), patindex('%[^0]%',cbd_accountNumber), 20),'-','')as AN ,cbd_bankCode as bkcode,COUNT(REPLACE(substring(LTRIM(RTRIM(cbd_accountNumber)), patindex('%[^0]%',cbd_accountNumber), 20),'-','')) as Occurance FROM tbl_trans_contactBankDetails GROUP BY REPLACE(substring(LTRIM(RTRIM(cbd_accountNumber)), patindex('%[^0]%',cbd_accountNumber), 20),'-',''),cbd_bankCode HAVING COUNT(REPLACE(substring(LTRIM(RTRIM(cbd_accountNumber)), patindex('%[^0]%',cbd_accountNumber), 20),'-',''))>1) as CC", "Distinct cbd_id,isnull(cnt_firstname,'')+' '+isnull(cnt_middlename,'')+' '+isnull(cnt_lastName,'')+'['+ isnull(cnt_UCC,'')+']' as CustomerName,cbd_accountName as AccountName,cbd_accountType as AccountType,cbd_accountCategory as AccountCategory,cbd_bankcode,REPLACE(substring(LTRIM(RTRIM(cbd_accountNumber)), patindex('%[^0]%',cbd_accountNumber), 20),'-','') as AccountNumber,bnk_bankName as BankName,bnk_branchName as BranchName,bnk_micrno as MICR ", "cbd_cntID=cnt_internalID and bnk_id=cbd_bankCode AND CC.bkcode=tbl_trans_contactBankDetails.cbd_bankCode AND CC.AN=REPLACE(substring(LTRIM(RTRIM(cbd_accountNumber)), patindex('%[^0]%',cbd_accountNumber), 20),'-','')");
                if (dt2.Rows.Count > 0)
                {
                    GridView1.DataSource = dt2;
                    GridView1.DataBind();
                    txtAccountNumber.Text = "";
                }
                else
                {
                    GridView1.DataSource = dt2;
                    GridView1.DataBind();
                    txtAccountNumber.Text = "";

                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "script18", "<Script Language='JavaScript'>alert('No Records Found!')</script> ");
                }


            }
        }
        protected void BindDetailData()
        {
            string AccountNumber = txtAccountNumber.Text.ToString();
            DataTable dt3 = new DataTable();
            if (AccountNumber != "" && AccountNumber == "")
            {
                dt3 = odbengine.GetDataTable("tbl_trans_contactBankDetails,tbl_master_contact,tbl_master_bank", "cbd_id,isnull(cnt_firstname,'')+' '+isnull(cnt_middlename,'')+' '+isnull(cnt_lastName,'')+'['+ isnull(cnt_UCC,'')+']' as CustomerName,cbd_accountName as AccountName,cbd_accountType as AccountType,cbd_accountCategory as AccountCategory,REPLACE(substring(LTRIM(RTRIM(cbd_accountNumber)), patindex('%[^0]%',cbd_accountNumber), 20),'-','') as AccountNumber,bnk_bankName as BankName,bnk_branchName as BranchName,bnk_micrno as MICR", "cbd_cntID=cnt_internalID and bnk_id=cbd_bankCode and cbd_accountNumber='" + AccountNumber + "'");
            }
            GridView1.DataSource = dt3;
            GridView1.DataBind();
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "script15", "<Script Language='JavaScript'>height();</script> ");

        }
        protected void chkDuplicate_CheckedChanged(object sender, EventArgs e)
        {
            if (chkDuplicate.Checked == true)
            {
                txtAccountNumber.Enabled = false;
            }
            else
            {
                txtAccountNumber.Enabled = true;
                DataTable dt3 = new DataTable();
                GridView1.DataSource = dt3;
                GridView1.DataBind();

            }
        }
    }
}