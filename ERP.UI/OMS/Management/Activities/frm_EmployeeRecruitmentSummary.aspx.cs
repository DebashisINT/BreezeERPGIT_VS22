using System;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI.WebControls;
using BusinessLogicLayer;
namespace ERP.OMS.Management.Activities
{
    public partial class management_activities_frm_EmployeeRecruitmentSummary : ERP.OMS.ViewState_class.VSPage
    {
        //DBEngine oDBEngine = new DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);
       // BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationManager.AppSettings["DBConnectionDefault"]);   MULTI

        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();

        DateTime Today = new DateTime();
        DateTime thisWeek_s = new DateTime();
        DateTime LastWeek_s = new DateTime();
        DateTime LastWeek_l = new DateTime();
        DateTime thisMonth_s = new DateTime();
        DateTime thisMonth_l = new DateTime();
        DateTime lastMonth_s = new DateTime();
        DateTime lastMonth_l = new DateTime();
        DateTime Year_s = new DateTime();
        DataTable DT = new DataTable();
        DataTable DT1 = new DataTable();
        DataTable DT2 = new DataTable();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //ShowSummaryTable();
            }
            MakeHeaderForSummaryTable();
            ShowSummaryTable();

        }
        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //'http://localhost:2957/InfluxCRM/management/testProjectMainPage_employee.aspx'
                string sPath = HttpContext.Current.Request.Url.ToString();
                oDBEngine.Call_CheckPageaccessebility(sPath);
            }
        }

        private void MakeHeaderForSummaryTable()
        {
            //____________Datetime allocation__________//

            Today = System.DateTime.Today;
            thisWeek_s = System.DateTime.Today.AddDays((WeekDya() - 1));
            LastWeek_s = System.DateTime.Today.AddDays(-((WeekDya() - 1) + 7));
            LastWeek_l = System.DateTime.Today.AddDays(-WeekDya());
            thisMonth_s = System.DateTime.Today.AddDays(-(oDBEngine.GetDate().Day - 1));
            thisMonth_l = System.DateTime.Today.AddDays(+(System.DateTime.DaysInMonth(System.DateTime.Today.Year, System.DateTime.Today.Month) - System.DateTime.Today.Day));
            lastMonth_s = thisMonth_s.AddMonths(-1);
            lastMonth_l = thisMonth_s.AddDays(-1);
            DateTime.TryParse(("01/01/" + System.DateTime.Today.Year.ToString()), out Year_s);
            //____________Header of table______________//
            TableCell tblecell = new TableCell();
            tblecell.Text = "Title";
            tblecell.Font.Bold = true;
            tblecell.Height = 30;
            tblecell.ForeColor = Color.FromName("White");

            TableRow rowHead = new TableRow();
            rowHead.BackColor = Color.FromName("#0F439D");
            rowHead.Cells.Add(tblecell);

            TableCell tblecel2 = new TableCell();
            tblecel2.Text = "Today";
            tblecel2.Font.Bold = true;
            tblecell.Height = 30;
            tblecel2.ForeColor = Color.FromName("White");
            rowHead.Cells.Add(tblecel2);

            TableCell tblecel3 = new TableCell();
            tblecel3.Text = "ThisWeek";
            tblecel3.Font.Bold = true;
            tblecell.Height = 30;
            tblecel3.ForeColor = Color.FromName("White");
            rowHead.Cells.Add(tblecel3);

            TableCell tblecel4 = new TableCell();
            tblecel4.Text = "LastWeek";
            tblecel4.Font.Bold = true;
            tblecell.Height = 30;
            tblecel4.ForeColor = Color.FromName("White");
            rowHead.Cells.Add(tblecel4);

            TableCell tblecel5 = new TableCell();
            tblecel5.Text = "ThisMonth";
            tblecel5.Font.Bold = true;
            tblecell.Height = 30;
            tblecel5.ForeColor = Color.FromName("White");
            rowHead.Cells.Add(tblecel5);

            TableCell tblecel6 = new TableCell();
            tblecel6.Text = "LastMonth";
            tblecel6.Font.Bold = true;
            tblecell.Height = 30;
            tblecel6.ForeColor = Color.FromName("White");
            rowHead.Cells.Add(tblecel6);

            TableCell tblecel7 = new TableCell();
            tblecel7.Text = "ThisYear";
            tblecel7.Font.Bold = true;
            tblecell.Height = 30;
            tblecel7.ForeColor = Color.FromName("White");
            rowHead.Cells.Add(tblecel7);

            TblSummary.Rows.Add(rowHead);

            //ShowSummaryTable();
        }
        private int WeekDya()
        {
            int days = 0;
            switch (System.DateTime.Today.DayOfWeek)
            {
                case DayOfWeek.Monday:
                    days = 1;
                    break;
                case DayOfWeek.Tuesday:
                    days = 2;
                    break;
                case DayOfWeek.Wednesday:
                    days = 3;
                    break;
                case DayOfWeek.Thursday:
                    days = 4;
                    break;
                case DayOfWeek.Friday:
                    days = 5;
                    break;
                case DayOfWeek.Saturday:
                    days = 6;
                    break;
                case DayOfWeek.Sunday:
                    days = 7;
                    break;
            }

            return days;
        }

        private void ShowSummaryTable()
        {

            //DBEngine oDBEngine = new DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);
            DT = oDBEngine.GetDataTable("tbl_trans_Activies", "tbl_trans_Activies.act_id AS Id,tbl_trans_Activies.act_scheduledDate AS ScheduleDate", "tbl_trans_Activies.act_activityType = 7 And tbl_trans_Activies.act_assignedTo = " + HttpContext.Current.Session["userid"]);
            //____________new Activity____________//
            if (DT.Rows.Count != 0)
            {
                DT1.Dispose();
                DT1 = new DataTable();
                DataColumn col1 = new DataColumn("Id");
                DataColumn col2 = new DataColumn("ScheduleDate");
                DT1.Columns.Add(col1);
                DT1.Columns.Add(col2);
                for (int i = 0; i < DT.Rows.Count; i++)
                {
                    DT2 = oDBEngine.GetDataTable("tbl_trans_Recruitment", "rd_Id", "rd_ActivityId = " + DT.Rows[i][0]);
                    if (DT2.Rows.Count == 0)
                    {
                        DataRow rownew = DT1.NewRow();
                        rownew[0] = DT.Rows[i][0];
                        rownew[1] = DT.Rows[i][1];
                        DT1.Rows.Add(rownew);
                    }
                }
                if (DT1.Rows.Count != 0)
                {
                    FillTable("New Activity");
                }
            }

            //____________Pending Activity____________//
            if (DT.Rows.Count != 0)
            {
                DT1.Dispose();
                DT1 = new DataTable();
                DataColumn col1 = new DataColumn("Id");
                DataColumn col2 = new DataColumn("ScheduleDate");
                DT1.Columns.Add(col1);
                DT1.Columns.Add(col2);
                for (int i = 0; i < DT.Rows.Count; i++)
                {
                    DT2 = oDBEngine.GetDataTable("tbl_trans_Recruitment", "rd_Id", "rd_ActivityId = " + DT.Rows[i][0]);
                    if (DT2.Rows.Count > 0)
                    {
                        DataRow rownew = DT1.NewRow();
                        rownew[0] = DT.Rows[i][0];
                        rownew[1] = DT.Rows[i][1];
                        DT1.Rows.Add(rownew);
                    }
                }
                if (DT1.Rows.Count != 0)
                {
                    FillTable("Pending Activity");
                }
            }

            //____________Pending Interviews____________//

            DT1 = oDBEngine.GetDataTable("tbl_trans_Activies ,tbl_trans_Recruitment,tbl_trans_RecruitmentDetail, tbl_trans_Interview", "tbl_trans_Activies.act_id AS Id, tbl_trans_Activies.act_scheduledDate AS ScheduleDate", "tbl_trans_Interview.int_InterviewOutcome = 0 And tbl_trans_Activies.act_actualEndDate IS NULL And tbl_trans_Activies.act_activityType = 7 And tbl_trans_Activies.act_assignedTo =" + HttpContext.Current.Session["userid"] + " and tbl_trans_Activies.act_id = tbl_trans_Recruitment.rd_ActivityId and tbl_trans_Recruitment.rd_ActivityId = tbl_trans_RecruitmentDetail.rde_Activityid and tbl_trans_RecruitmentDetail.rde_Activityid = tbl_trans_Interview.int_ActivityId AND    tbl_trans_RecruitmentDetail.rde_Id = tbl_trans_Interview.int_InternalId ");
            if (DT1.Rows.Count != 0)
            {
                FillTable("Pending Interview");
            }

            //____________Completed Interviews____________//

            DT1 = oDBEngine.GetDataTable("tbl_trans_Activies ,tbl_trans_Recruitment,tbl_trans_RecruitmentDetail, tbl_trans_Interview", "tbl_trans_Activies.act_id AS Id, tbl_trans_Activies.act_scheduledDate AS ScheduleDate", "tbl_trans_Interview.int_InterviewOutcome <> 0 And tbl_trans_Activies.act_actualEndDate IS NULL And tbl_trans_Activies.act_activityType = 7 And tbl_trans_Activies.act_assignedTo =" + HttpContext.Current.Session["userid"] + " and tbl_trans_Activies.act_id = tbl_trans_Recruitment.rd_ActivityId and tbl_trans_Recruitment.rd_ActivityId = tbl_trans_RecruitmentDetail.rde_Activityid and tbl_trans_RecruitmentDetail.rde_Activityid = tbl_trans_Interview.int_ActivityId AND    tbl_trans_RecruitmentDetail.rde_Id = tbl_trans_Interview.int_InternalId ");
            if (DT1.Rows.Count != 0)
            {
                FillTable("Completed Interview");
            }

            //____________Hired Recuritment____________//

            DT1 = oDBEngine.GetDataTable("tbl_trans_Activies, tbl_trans_RecruitmentDetail, tbl_trans_Recruitment", "tbl_trans_Activies.act_id, tbl_trans_Activies.act_scheduledDate AS ScheduleDate", "tbl_trans_Activies.act_actualEndDate IS NULL And tbl_trans_Activies.act_activityType = 7 And tbl_trans_RecruitmentDetail.rde_EmployementConfirmed = 1 And tbl_trans_Activies.act_assignedTo =123 and tbl_trans_Activies.act_id = tbl_trans_RecruitmentDetail.rde_Activityid and tbl_trans_RecruitmentDetail.rde_Activityid = tbl_trans_Recruitment.rd_ActivityId");
            if (DT1.Rows.Count != 0)
            {
                FillTable("Hired Candidate");
            }

            //____________Elininated Candidate____________//

            DT1 = oDBEngine.GetDataTable("tbl_trans_Activies ,tbl_trans_Recruitment,tbl_trans_RecruitmentDetail, tbl_trans_Interview", "tbl_trans_Activies.act_id AS Id, tbl_trans_Activies.act_scheduledDate AS ScheduleDate", "tbl_trans_Interview.int_InterviewOutcome = 2 And tbl_trans_Activies.act_actualEndDate IS NULL And tbl_trans_Activies.act_activityType = 7 And tbl_trans_Activies.act_assignedTo =" + HttpContext.Current.Session["userid"] + " and tbl_trans_Activies.act_id = tbl_trans_Recruitment.rd_ActivityId and tbl_trans_Recruitment.rd_ActivityId = tbl_trans_RecruitmentDetail.rde_Activityid and tbl_trans_RecruitmentDetail.rde_Activityid = tbl_trans_Interview.int_ActivityId AND    tbl_trans_RecruitmentDetail.rde_Id = tbl_trans_Interview.int_InternalId ");
            if (DT1.Rows.Count != 0)
            {
                FillTable("Eliminated Candidate");
            }
        }

        private void FillTable(string activity)
        {
            DataRow[] SubItem;
            TableRow rowNew = new TableRow();
            TableCell newcell = new TableCell();
            newcell.Text = activity;
            rowNew.Cells.Add(newcell);

            //______For Today _______//

            String Expression = "ScheduleDate = #" + Today + "#";
            SubItem = DT1.Select(Expression);
            TableCell newcell1 = new TableCell();
            newcell1.Text = SubItem.Length.ToString();
            rowNew.Cells.Add(newcell1);

            //______For This Week _______//

            Expression = "ScheduleDate >= #" + thisWeek_s + "# And " + "ScheduleDate <= #" + Today + "#";
            SubItem = DT1.Select(Expression);
            TableCell newcell2 = new TableCell();
            newcell2.Text = SubItem.Length.ToString();
            rowNew.Cells.Add(newcell2);

            //______For Last Week _______//

            Expression = "ScheduleDate >= #" + LastWeek_s + "# And " + "ScheduleDate <= #" + LastWeek_l + "#";
            SubItem = DT1.Select(Expression);
            TableCell newcell3 = new TableCell();
            newcell3.Text = SubItem.Length.ToString();
            rowNew.Cells.Add(newcell3);

            //______For THIS MONTH _______//

            Expression = "ScheduleDate >= #" + thisMonth_s + "# And " + "ScheduleDate <= #" + thisMonth_l + "#";
            SubItem = DT1.Select(Expression);
            TableCell newcell4 = new TableCell();
            newcell4.Text = SubItem.Length.ToString();
            rowNew.Cells.Add(newcell4);

            //______For LAST MONTH _______//

            Expression = "ScheduleDate >= #" + lastMonth_s + "# And " + "ScheduleDate <= #" + lastMonth_l + "#";
            SubItem = DT1.Select(Expression);
            TableCell newcell5 = new TableCell();
            newcell5.Text = SubItem.Length.ToString();
            rowNew.Cells.Add(newcell5);

            //______For THIS YEAR _______//

            Expression = "ScheduleDate >= #" + Convert.ToDateTime(Year_s) + "# And " + "ScheduleDate <=#" + Convert.ToDateTime(Today) + "#";
            SubItem = DT1.Select(Expression);
            TableCell newcell6 = new TableCell();
            newcell6.Text = SubItem.Length.ToString();
            rowNew.Cells.Add(newcell6);



            TblSummary.Rows.Add(rowNew);

        }

    }
}