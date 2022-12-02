using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ERP.OMS.Management.Master
{
    public partial class WorkinhHourAddEdit : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["id"] == "Add")
                {
                    hdAddedit.Value = "Add";
                }
                else
                {
                    hdAddedit.Value = "Edit";
                    txtName.ClientEnabled = false;
                    hdId.Value = Convert.ToString(Request.QueryString["id"]);
                    Session["RoasterID"] = Convert.ToString(Request.QueryString["id"]);

                    ProcedureExecute proc = new ProcedureExecute("Prc_WorkingHourAddEdit");
                    proc.AddVarcharPara("@Action", 100, "GetDetails");
                    proc.AddVarcharPara("@Id", 100, Convert.ToString(Request.QueryString["id"]));
                    DataSet ds = proc.GetDataSet();
                    txtName.Text = ds.Tables[0].Rows[0]["Name"].ToString();

                    foreach (DataRow dr in ds.Tables[1].Rows) 
                    {
                        if (dr["DayWeek"].ToString().Trim() == "1")
                        {
                            chkSunday.Checked = true;
                            beginSunday.DateTime = DateTime.ParseExact(dr["BeginTime"].ToString(), "H:mm:ss", null, System.Globalization.DateTimeStyles.None);
                            endSunday.DateTime = DateTime.ParseExact(dr["EndTime"].ToString(), "H:mm:ss", null, System.Globalization.DateTimeStyles.None);
                            graceSunday.Text = Convert.ToString(dr["Grace"]);
                        }
                        if (dr["DayWeek"].ToString().Trim() == "2")
                        {
                            chkMonday.Checked = true;
                            beginMonday.DateTime = DateTime.ParseExact(dr["BeginTime"].ToString(), "H:mm:ss", null, System.Globalization.DateTimeStyles.None);
                            Endmonday.DateTime = DateTime.ParseExact(dr["EndTime"].ToString(), "H:mm:ss", null, System.Globalization.DateTimeStyles.None);
                            graceMonday.Text = Convert.ToString(dr["Grace"]);
                        }
                        if (dr["DayWeek"].ToString().Trim() == "3")
                        {
                            chktuesday.Checked = true;
                            BeginTuesDay.DateTime = DateTime.ParseExact(dr["BeginTime"].ToString(), "H:mm:ss", null, System.Globalization.DateTimeStyles.None);
                            endTuesDay.DateTime = DateTime.ParseExact(dr["EndTime"].ToString(), "H:mm:ss", null, System.Globalization.DateTimeStyles.None);
                            graceTuesday.Text = Convert.ToString(dr["Grace"]);
                        }
                        if (dr["DayWeek"].ToString().Trim() == "4")
                        {
                            chkWednesday.Checked = true;
                            beginWednesday.DateTime = DateTime.ParseExact(dr["BeginTime"].ToString(), "H:mm:ss", null, System.Globalization.DateTimeStyles.None);
                            endWednesday.DateTime = DateTime.ParseExact(dr["EndTime"].ToString(), "H:mm:ss", null, System.Globalization.DateTimeStyles.None);
                            graceWednesday.Text = Convert.ToString(dr["Grace"]);
                        }
                        if (dr["DayWeek"].ToString().Trim() == "5")
                        {
                            chkThursday.Checked = true;
                            beginThursday.DateTime = DateTime.ParseExact(dr["BeginTime"].ToString(), "H:mm:ss", null, System.Globalization.DateTimeStyles.None);
                            endThursday.DateTime = DateTime.ParseExact(dr["EndTime"].ToString(), "H:mm:ss", null, System.Globalization.DateTimeStyles.None);
                            gracethursday.Text = Convert.ToString(dr["Grace"]);
                        }
                        if (dr["DayWeek"].ToString().Trim() == "6")
                        {
                            chkFriday.Checked = true;
                            beginFriday.DateTime = DateTime.ParseExact(dr["BeginTime"].ToString(), "H:mm:ss", null, System.Globalization.DateTimeStyles.None);
                            endFriday.DateTime = DateTime.ParseExact(dr["EndTime"].ToString(), "H:mm:ss", null, System.Globalization.DateTimeStyles.None);
                            graceFridDay.Text = Convert.ToString(dr["Grace"]);
                        }
                        if (dr["DayWeek"].ToString().Trim() == "7")
                        {
                            chkSaturday.Checked = true;
                            beginSaturday.DateTime = DateTime.ParseExact(dr["BeginTime"].ToString(), "H:mm:ss", null, System.Globalization.DateTimeStyles.None);
                            endSaturday.DateTime = DateTime.ParseExact(dr["EndTime"].ToString(), "H:mm:ss", null, System.Globalization.DateTimeStyles.None);
                            graceSaturday.Text = Convert.ToString(dr["Grace"]);
                        }
                    
                    }

                     


                }
            }


        }

        protected void Save_Click(object sender, EventArgs e)
        {
            DataTable RosterDt = new DataTable();
            RosterDt.Columns.Add("Day", typeof(System.String));
            RosterDt.Columns.Add("BeginTime", typeof(System.DateTime));
            RosterDt.Columns.Add("EndTime", typeof(System.DateTime));
            RosterDt.Columns.Add("Grace", typeof(System.String));

            DataRow WeekRow = RosterDt.NewRow();
             
            if (chkSunday.Checked) {

                if (beginSunday.DateTime.Year == 100)
                    beginSunday.DateTime = beginSunday.DateTime.AddMinutes(1);

                if (endSunday.DateTime.Year == 100)
                    endSunday.DateTime = endSunday.DateTime.AddMinutes(1);


                WeekRow["Day"] = "1";
                WeekRow["BeginTime"] = beginSunday.DateTime;
                WeekRow["EndTime"] = endSunday.DateTime;
                WeekRow["Grace"] = graceSunday.Text;
                RosterDt.Rows.Add(WeekRow);
            }

            if (chkMonday.Checked)
            {
                if (beginMonday.DateTime.Year == 100)
                    beginMonday.DateTime = beginMonday.DateTime.AddMinutes(1);

                if (Endmonday.DateTime.Year == 100)
                    Endmonday.DateTime = Endmonday.DateTime.AddMinutes(1);


                WeekRow = RosterDt.NewRow();
                WeekRow["Day"] = "2";
                WeekRow["BeginTime"] = beginMonday.DateTime;
                WeekRow["EndTime"] = Endmonday.DateTime;
                WeekRow["Grace"] = graceMonday.Text;
                RosterDt.Rows.Add(WeekRow);
            }
            if (chktuesday.Checked)
            {
                if (BeginTuesDay.DateTime.Year == 100)
                    BeginTuesDay.DateTime = BeginTuesDay.DateTime.AddMinutes(1);

                if (endTuesDay.DateTime.Year == 100)
                    endTuesDay.DateTime = endTuesDay.DateTime.AddMinutes(1);


                WeekRow = RosterDt.NewRow();
                WeekRow["Day"] = "3";
                WeekRow["BeginTime"] = BeginTuesDay.DateTime;
                WeekRow["EndTime"] = endTuesDay.DateTime;
                WeekRow["Grace"] = graceTuesday.Text;
                RosterDt.Rows.Add(WeekRow);
            }
            if (chkWednesday.Checked)
            {
                if (beginWednesday.DateTime.Year == 100)
                    beginWednesday.DateTime = beginWednesday.DateTime.AddMinutes(1);

                if (endWednesday.DateTime.Year == 100)
                    endWednesday.DateTime = endWednesday.DateTime.AddMinutes(1);


                WeekRow = RosterDt.NewRow();
                WeekRow["Day"] = "4";
                WeekRow["BeginTime"] = beginWednesday.DateTime;
                WeekRow["EndTime"] = endWednesday.DateTime;
                WeekRow["Grace"] = graceWednesday.Text;
                RosterDt.Rows.Add(WeekRow);
            }
            if (chkThursday.Checked)
            {
                if (beginThursday.DateTime.Year == 100)
                    beginThursday.DateTime = beginThursday.DateTime.AddMinutes(1);

                if (endThursday.DateTime.Year == 100)
                    endThursday.DateTime = endThursday.DateTime.AddMinutes(1);


                WeekRow = RosterDt.NewRow();
                WeekRow["Day"] = "5";
                WeekRow["BeginTime"] = beginThursday.DateTime;
                WeekRow["EndTime"] = endThursday.DateTime;
                WeekRow["Grace"] = gracethursday.Text;
                RosterDt.Rows.Add(WeekRow);
            }
            if (chkFriday.Checked)
            {
                if (beginFriday.DateTime.Year == 100)
                    beginFriday.DateTime = beginFriday.DateTime.AddMinutes(1);

                if (endFriday.DateTime.Year == 100)
                    endFriday.DateTime = endFriday.DateTime.AddMinutes(1);


                WeekRow = RosterDt.NewRow();
                WeekRow["Day"] = "6";
                WeekRow["BeginTime"] = beginFriday.DateTime;
                WeekRow["EndTime"] = endFriday.DateTime;
                WeekRow["Grace"] = graceFridDay.Text;
                RosterDt.Rows.Add(WeekRow);
            }
            if (chkSaturday.Checked)
            {
                if (beginSaturday.DateTime.Year == 100)
                    beginSaturday.DateTime = beginSaturday.DateTime.AddMinutes(1);

                if (endSaturday.DateTime.Year == 100)
                    endSaturday.DateTime = endSaturday.DateTime.AddMinutes(1);



                WeekRow = RosterDt.NewRow();
                WeekRow["Day"] = "7";
                WeekRow["BeginTime"] = beginSaturday.DateTime;
                WeekRow["EndTime"] = endSaturday.DateTime;
                WeekRow["Grace"] = graceSaturday.Text;
                RosterDt.Rows.Add(WeekRow);
            }

             

            try
            {
                ProcedureExecute proc = new ProcedureExecute("Prc_WorkingHourAddEdit");
                proc.AddVarcharPara("@Action", 100, hdAddedit.Value);
                proc.AddVarcharPara("@name", 100, txtName.Text);
                proc.AddVarcharPara("@Id", 100,Convert.ToString(Session["RoasterID"]));
                proc.AddPara("@dayList", RosterDt);
                
                proc.RunActionQuery();
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "SavedSuccessfull()", true);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "text", "ErrorShow('" + ex.Message + "')", true);
            }
            

        }
    }
}