using System;
using System.Data;
using System.Web.UI;
using System.Configuration;
using BusinessLogicLayer;

namespace ERP.OMS.Reports
{
    public partial class Reports_frmMyPage : System.Web.UI.Page
    {
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);
        string Contactid = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                HpAttendance.Attributes.Add("onclick", "frmOpenNewWindow1('frm_Attendance_FromMyPage.aspx?','Attendance Register','950px','450px')");
                HpMarkAttendance.Attributes.Add("onclick", "frmOpenNewWindow1('frm_Attendance_EmployeeWise_fromMyPage.aspx?','Mark Attendance ','950px','450px')");
                HpLeaveRegister.Attributes.Add("onclick", "frmOpenNewWindow1('frmReport_LeaveBalance_FromMyPage.aspx?','Leave Register ','950px','450px')");
            }
            FillDetails();
            //string a = Session["userid"].ToString();
           // Page.ClientScript.RegisterStartupScript(GetType(), "Height", "<script language='Javascript'>height()</script>");
        }
        public void FillDetails()
        {
            Contactid = oDBEngine.GetFieldValue("tbl_master_user", "user_contactid", "user_id='" + Session["userid"].ToString() + "'", 1)[0, 0];
            string Address = oDBEngine.GetFieldValue("tbl_master_address", " top 1 isnull(add_address1,'')+' '+isnull(add_address2,'')+' '+isnull(add_address3,'')as address", "add_cntid='" + Contactid.ToString() + "'", 1)[0, 0];
            lblAdd.Text = Address.ToString();
            string[,] Name = oDBEngine.GetFieldValue("tbl_master_contact", "isnull(cnt_firstname,'')+' '+isnull(cnt_middlename,'')+' '+isnull(cnt_lastname,'') as name,cnt_ucc", "cnt_internalid='" + Contactid.ToString() + "'", 2);
            lblName.Text = Name[0, 0].ToString();
            lblCode.Text = Name[0, 1].ToString();
            DataTable PhoneNo = oDBEngine.GetDataTable("tbl_master_phonefax", " isnull(phf_countrycode,'')+isnull(phf_areacode,'')+isnull(phf_phonenumber,'') as landnumber,phf_type from tbl_master_phonefax where phf_type='Residence' and phf_cntid like '" + Contactid.ToString() + "' union select isnull(phf_countrycode,'')+isnull(phf_areacode,'')+isnull(phf_phonenumber,'') as landnumber,phf_type from tbl_master_phonefax WHERE phf_type='mobile' and phf_cntid='" + Contactid.ToString() + "' union select isnull(phf_countrycode,'')+isnull(phf_areacode,'')+isnull(phf_phonenumber,'') as landnumber,phf_type", "phf_type='office' and phf_cntid='" + Contactid.ToString() + "'");
            if (PhoneNo.Rows.Count == 1)
            {
                lblPhone.Text = PhoneNo.Rows[0][1].ToString() + " : " + PhoneNo.Rows[0][0].ToString();
            }
            if (PhoneNo.Rows.Count == 2)
            {
                lblPhone.Text = PhoneNo.Rows[0][1].ToString() + " : " + PhoneNo.Rows[0][0].ToString() + "; " + PhoneNo.Rows[1][1].ToString() + " : " + PhoneNo.Rows[1][0].ToString();
            }
            if (PhoneNo.Rows.Count == 3)
            {
                lblPhone.Text = PhoneNo.Rows[0][1].ToString() + " : " + PhoneNo.Rows[0][0].ToString() + "; " + PhoneNo.Rows[1][1].ToString() + " : " + PhoneNo.Rows[1][0].ToString() + PhoneNo.Rows[2][1].ToString() + " : " + PhoneNo.Rows[2][0].ToString();
            }
            DataTable Emailid = oDBEngine.GetDataTable("tbl_master_email", " eml_email,eml_type from tbl_master_email where eml_cntid='" + Contactid.ToString() + "' and eml_type='official' union select eml_email,eml_type from tbl_master_email where eml_cntid='" + Contactid.ToString().Trim() + "' and eml_type='Personal' union select eml_email,eml_type", "eml_cntid='" + Contactid.ToString() + "' and eml_type='Web Site'");
            if (Emailid.Rows.Count == 1)
            {
                lblEmail.Text = Emailid.Rows[0][1].ToString() + " : " + Emailid.Rows[0][0].ToString();
            }
            if (PhoneNo.Rows.Count == 2)
            {
                lblEmail.Text = Emailid.Rows[0][1].ToString() + ":" + Emailid.Rows[0][0].ToString() + ";" + Emailid.Rows[1][1].ToString() + ":" + Emailid.Rows[1][0].ToString();
            }
            if (PhoneNo.Rows.Count == 3)
            {
                lblEmail.Text = Emailid.Rows[0][1].ToString() + ":" + PhoneNo.Rows[0][0].ToString() + ";" + Emailid.Rows[1][1].ToString() + ":" + Emailid.Rows[1][0].ToString() + Emailid.Rows[2][1].ToString() + ":" + PhoneNo.Rows[2][0].ToString();
            }
            string Photo = oDBEngine.GetFieldValue("tbl_master_document as doc inner join tbl_master_documentType as dcotype on doc.doc_documentTypeId=dcotype.dty_id ", "doc.doc_source", " dty_documenttype='Photograph' and doc.doc_contactid='" + Contactid.ToString() + "'", 1)[0, 0];
            if (Photo.ToString() != "n")
            {
                iPhoto.ImageUrl = Photo.ToString();
            }
            lblCompany.Text = oDBEngine.GetFieldValue("(select emp_organization from tbl_trans_employeectc where emp_cntid='" + Contactid.ToString().Trim() + "' and (emp_effectiveuntil is null or emp_effectiveuntil='1900-01-01 00:00:00.000')) as a inner join tbl_master_company as com on com.cmp_id=a.emp_organization", "com.cmp_name", null, 1)[0, 0];
            lblBranch.Text = oDBEngine.GetFieldValue("tbl_master_branch", "branch_description+'['+ltrim(rtrim(branch_code))+']' as BranchName", "branch_id in(select ltrim(rtrim(cnt_branchid)) from tbl_master_contact where cnt_internalid='" + Contactid.ToString().Trim() + "')", 1)[0, 0];

        }
    }
}