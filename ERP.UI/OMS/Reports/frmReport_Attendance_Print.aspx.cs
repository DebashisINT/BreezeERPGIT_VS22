using System;
using System.Data;
using CrystalDecisions.CrystalReports.Engine;
using System.Configuration;
using BusinessLogicLayer;

namespace ERP.OMS.Reports
{
    public partial class Reports_frmReport_Attendance_Print : System.Web.UI.Page
    {
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);
        ReportDocument AttendenceReportDocu = new ReportDocument();
        protected void Page_Load(object sender, EventArgs e)
        {
            showCrystalReport();
        }
        public void showCrystalReport()
        {
            string[] data = Request.QueryString["id"].Split('~');
            string aa = data[2];
            string clasWhere = "emp.emp_contactId = cont.cnt_internalId and ctc.emp_cntId = emp.emp_contactId and comp.cmp_id = ctc.emp_Organization and branch.branch_id = cont.cnt_branchid and atd.atd_cntId = emp.emp_contactId and (emp.emp_dateOfLeaving is null OR emp.emp_dateOfLeaving='1/1/1900 12:00:00 AM' OR emp.emp_dateOfLeaving='1/1/1900' OR month(emp.emp_dateOfLeaving) >= " + data[1] + ")" +
                               " and month(emp.emp_dateofjoining) <= " + data[1];

            if (data[0] != "")
            {
                clasWhere = clasWhere + " and atd.atd_year =" + data[0];
            }
            if (data[1] != "")
            {
                clasWhere = clasWhere + " and atd.atd_Month =" + data[1];
            }
            if (data[3] != "All")
            {
                clasWhere = clasWhere + " and cont.cnt_branchid =" + data[3];
            }
            if (data[2] != "All")
            {
                clasWhere = clasWhere + " and comp.cmp_id =" + data[2];
            }
            if (data[4] != "All")
            {
                clasWhere = clasWhere + " and cont.cnt_internalId = '" + data[4] + "'";
            }
            DataTable DT = oDBEngine.GetDataTable(" tbl_master_contact cont,tbl_master_employee emp,tbl_trans_employeeCTC ctc,tbl_master_company comp,tbl_master_branch branch,tbl_trans_attendance atd ", " distinct atd_id, atd_cntId, atd_year, atd_Month,(Isnull(cont.cnt_firstName,'') +''+IsNull(cont.cnt_middleName,'') + ''+Isnull(cont.cnt_lastName,'')) as empName,isnull(cont.cnt_shortName,'') as code,comp.cmp_Name,branch.branch_description , IsNull(atd_StatusDay1,'*') day1, IsNull(atd_StatusDay2,'*') day2, IsNull(atd_StatusDay3,'*') day3, IsNull(atd_StatusDay4,'*') day4, IsNull(atd_StatusDay5,'*') day5, IsNull(atd_StatusDay6,'*') day6, IsNull(atd_StatusDay7,'*') day7, IsNull(atd_StatusDay8,'*') day8, IsNull(atd_StatusDay9,'*') day9, IsNull(atd_StatusDay10,'*') day10,  IsNull(atd_StatusDay11,'*') day11, IsNull(atd_StatusDay12,'*') day12, IsNull(atd_StatusDay13,'*') day13, IsNull(atd_StatusDay14,'*') day14, IsNull(atd_StatusDay15,'*') day15, IsNull(atd_StatusDay16,'*') day16, IsNull(atd_StatusDay17,'*') day17, IsNull(atd_StatusDay18,'*') day18, IsNull(atd_StatusDay19,'*') day19, IsNull(atd_StatusDay20,'*') day20, IsNull(atd_StatusDay21,'*') day21, IsNull(atd_StatusDay22,'*') day22, IsNull(atd_StatusDay23,'*') day23, IsNull(atd_StatusDay24,'*') day24, IsNull(atd_StatusDay25,'*') day25, IsNull(atd_StatusDay26,'*') day26, IsNull(atd_StatusDay27,'*') day27, IsNull(atd_StatusDay28,'*') day28, IsNull(atd_StatusDay29,'*') day29, IsNull(atd_StatusDay30,'*') day30, IsNull(atd_StatusDay31,'*') day31 ", clasWhere);

            string path = Server.MapPath("~\\Reports\\AttendenceRport.rpt");
            AttendenceReportDocu.Load(path);



            TextObject txtYear = (TextObject)AttendenceReportDocu.ReportDefinition.ReportObjects["txtYear"];
            if (data[0] != "")
            {
                txtYear.Text = data[0];
            }
            else
            {
                txtYear.Text = "--";
            }

            TextObject txtMonth = (TextObject)AttendenceReportDocu.ReportDefinition.ReportObjects["txtMonth"];
            if (data[1] != "")
            {
                txtMonth.Text = data[1];
            }
            else
            {
                txtMonth.Text = "--";
            }

            AttendenceReportDocu.Database.Tables["command"].SetDataSource(DT);
            CrystalReportViewer1.ReportSource = AttendenceReportDocu;
            CrystalReportViewer1.DataBind();
        }
    }
}