using System;
using System.Data;
using System.Web.UI;
using System.Configuration;
using BusinessLogicLayer;
namespace ERP.OMS.Reports
{
    public partial class Reports_frm_rptEmpBDaydtls : System.Web.UI.Page
    {
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        string RefreshReminderStr = "";
        protected void Page_Load(object sender, EventArgs e)
        {

            BindTable();
            Page.ClientScript.RegisterStartupScript(GetType(), "Height", "<script language='JavaScript'>height();</script>");
        }
        //protected void btnShow_Click(object sender, EventArgs e)
        //{
        //    //BindTable();

        //}
        public void BindTable()
        {
            string FromDate = oDBEngine.GetDate().ToString();
            FromDate = FromDate.Substring(0, 4);
            string[] FD = FromDate.Split('/');
            double NoOfDays = 0.0;
            if (txtDays.Text == "" || txtDays.Text == "Days")
            {
                NoOfDays = 30;
            }
            else
            {
                NoOfDays = Convert.ToDouble(txtDays.Text.ToString());
            }
            string AfterAdd = Convert.ToDateTime(oDBEngine.GetDate().AddDays(NoOfDays)).ToString();
            string MonthDate = AfterAdd.Substring(0, 4);
            string[] MonthDate1 = MonthDate.Split('/');
            DataTable DtMain = new DataTable();
            if (dpSelect.SelectedItem.Value == "1")
            {
                DtMain = oDBEngine.GetDataTable("(Select distinct dateadd(yy,datediff(yyyy,cnt_dob,getdate()),cnt_dob) as Compare,isnull(cnt_firstname,'')+isnull(cnt_middlename,'')+isnull(cnt_lastname,'') as Name,cnt_shortname as Code,(select deg_designation from  tbl_master_designation where tbl_master_designation.deg_id=b.emp_designation) as Designation,(select branch_description from tbl_master_branch where tbl_master_branch.branch_id=b.emp_branch)as Branch,(select cost_description from tbl_master_costCenter where tbl_master_costCenter.cost_id=b.emp_department)as Department,cnt_dob as DateOfBirth,(select top 1 isnull(phf_phonenumber,'') from tbl_master_phonefax where phf_type='Mobile' and tbl_master_phonefax.phf_cntid=b.emp_cntid) as PhoneNumber,(select top 1 eml_email from tbl_master_email where eml_type='Official' and tbl_master_email.eml_cntid=b.emp_cntid) as MailId from tbl_master_contact a inner join tbl_trans_employeectc b on a.cnt_internalid=b.emp_cntid WHERE  cnt_dob<>'1/1/1900 12:00:00 AM' and cnt_dob is not null and ( b.emp_effectiveuntil is null or b.emp_effectiveuntil='1/1/1900 12:00:00 AM'))as c", "cast(c.compare as datetime) AS comparedate,c.*", " c.Compare between  convert(varchar(10),getdate(),110) and DATEADD(day, " + NoOfDays.ToString() + ", getdate())");
            }
            else
            {
                DtMain = oDBEngine.GetDataTable("(Select distinct dateadd(yy,datediff(yyyy,cnt_anniversarydate,getdate()),cnt_anniversarydate) as Compare,isnull(cnt_firstname,'')+isnull(cnt_middlename,'')+isnull(cnt_lastname,'') as Name,cnt_shortname as Code,(select deg_designation from  tbl_master_designation where tbl_master_designation.deg_id=b.emp_designation) as Designation,(select branch_description from tbl_master_branch where tbl_master_branch.branch_id=b.emp_branch)as Branch,(select cost_description from tbl_master_costCenter where tbl_master_costCenter.cost_id=b.emp_department)as Department,cnt_anniversarydate as DateOfBirth,(select top 1 isnull(phf_phonenumber,'') from tbl_master_phonefax where phf_type='Mobile' and tbl_master_phonefax.phf_cntid=b.emp_cntid) as PhoneNumber,(select top 1 eml_email from tbl_master_email where eml_type='Official' and tbl_master_email.eml_cntid=b.emp_cntid) as MailId from tbl_master_contact a inner join tbl_trans_employeectc b on a.cnt_internalid=b.emp_cntid WHERE  cnt_dob<>'1/1/1900 12:00:00 AM' and cnt_anniversarydate is not null and ( b.emp_effectiveuntil is null or b.emp_effectiveuntil='1/1/1900 12:00:00 AM'))as c", "cast(c.compare as datetime) AS comparedate,c.*", " c.Compare between  convert(varchar(10),getdate(),110) and DATEADD(day, " + NoOfDays.ToString() + ", getdate())");

            }
            grdDetails.DataSource = DtMain;
            grdDetails.DataBind();
            if (dpSelect.SelectedItem.Value == "1")
            {
                grdDetails.Columns[5].Caption = "Date Of Birth";

            }
            else
            {
                grdDetails.Columns[5].Caption = "Marriage Anniversary";
            }
        }
        protected void grdDetails_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            if (e.Parameters == "PC")
            {
                RefreshReminderStr = "DATABIND";
                BindTable();
            }
            else
            {
                string EmpCodeForReminder = "";
                EmpCodeForReminder = e.Parameters.ToString();
                RefreshReminderStr = "REMINDER";
                SetReminder(EmpCodeForReminder);

            }
        }
        public void SetReminder(string empcode)
        {
            string SourceId = "";
            if (dpSelect.SelectedItem.Value == "1")
            {
                SourceId = "B" + empcode;

            }
            else
            {
                SourceId = "M" + empcode;

            }
            string[,] Rem_Id = oDBEngine.GetFieldValue("tbl_trans_reminder", "rem_id", "rem_sourceid='" + SourceId.ToString() + "' and year(rem_startdate)=year(getdate())", 1);
            string RemId = "";
            if (Rem_Id[0, 0] != "n")
            {
                RemId = Rem_Id[0, 0];
            }
            string Startdate = "";
            if (dpSelect.SelectedItem.Value == "1")
            {
                Startdate = oDBEngine.GetFieldValue("tbl_master_contact", "dateadd(yy,datediff(yyyy,cnt_dob,getdate()),cnt_dob) as startdate", "cnt_shortname='" + empcode.ToString().Trim() + "'", 1)[0, 0];
            }
            else
            {
                Startdate = oDBEngine.GetFieldValue("tbl_master_contact", "dateadd(yy,datediff(yyyy,cnt_anniversarydate,getdate()),cnt_dob) as startdate", "cnt_shortname='" + empcode.ToString().Trim() + "'", 1)[0, 0];

            }
            string StartTime = " 08:00";
            string[] StartDatePart = Startdate.Split('/');
            string[] st = StartDatePart[2].ToString().Split(' ');
            Startdate = StartDatePart[0].ToString() + "/" + StartDatePart[1].ToString() + "/" + st[0].ToString().Trim();
            Startdate = Startdate + StartTime;
            string Endtime = " 23:59";
            string Enddate = StartDatePart[0].ToString() + "/" + StartDatePart[1].ToString() + "/" + st[0].ToString().Trim() + Endtime;
            string note = "";
            string Empname = "";
            Empname = oDBEngine.GetFieldValue("tbl_master_contact", "isnull(cnt_firstname,'')+isnull(cnt_middlename,'')+isnull(cnt_lastname,'') as Name", "cnt_shortname='" + empcode.ToString().Trim() + "'", 1)[0, 0];
            if (dpSelect.SelectedItem.Value == "1")
            {
                SourceId = "B" + empcode;
                note = "Today is Birthday of " + Empname.ToString().Trim() + "[" + empcode.ToString().Trim() + "]";
            }
            else
            {
                SourceId = "M" + empcode;
                note = "Today is Marrage Anniversary of " + Empname.ToString().Trim() + "(" + empcode.ToString().Trim() + ")";
            }
            if (RemId == "")
            {
                oDBEngine.InsurtFieldValue("tbl_trans_reminder", "rem_createUser,rem_createDate,rem_targetUser,rem_startDate,rem_endDate,rem_reminderContent,rem_displayTricker,rem_actionTaken,rem_sourceid,CreateDate,CreateUser", "'" + Session["userid"].ToString() + "','" + oDBEngine.GetDate().ToShortDateString() + "','" + Session["userid"].ToString() + "','" + Startdate + "','" + Enddate + "' ,'" + note.ToString().Trim() + "','1','0','" + SourceId.ToString() + "','" + oDBEngine.GetDate().ToShortDateString() + "','" + Session["userid"].ToString() + "'");
            }
            else
            {
                RefreshReminderStr = "ALREADYEXIST";
            }
        }
        protected void grdDetails_CustomJSProperties(object sender, DevExpress.Web.ASPxGridViewClientJSPropertiesEventArgs e)
        {
            e.Properties["cpa"] = RefreshReminderStr;
        }
    }
}