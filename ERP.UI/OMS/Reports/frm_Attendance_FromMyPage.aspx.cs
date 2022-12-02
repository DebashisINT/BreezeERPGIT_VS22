using System.Web.UI.WebControls;
using System;
using System.Configuration;
using System.Web;
using System.Web.UI;
using BusinessLogicLayer;
namespace ERP.OMS.Reports
{
    public partial class Reports_frm_Attendance_FromMyPage : System.Web.UI.Page
    {
        DBEngine oDBEngine = new DBEngine();
        ClsDropDownlistNameSpace.clsDropDownList cls = new ClsDropDownlistNameSpace.clsDropDownList();
        public string pageAccess = "";
        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //'http://localhost:2957/InfluxCRM/management/testProjectMainPage_employee.aspx'
                string sPath = HttpContext.Current.Request.Url.ToString();
                oDBEngine.Call_CheckPageaccessebility(sPath);
            }
        }
        protected void Page_Init(object sender, EventArgs e)
        {
            SDSAttdMain.ConnectionString = Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]);
        }
        protected void Page_Load(object sender, EventArgs e)
        {

            if (HttpContext.Current.Session["userid"] == null)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }

            //this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>height();</script>");
            if (!IsPostBack)
            {
                string Contactid = oDBEngine.GetFieldValue("tbl_master_user", "user_contactid", "user_id='" + Session["userid"].ToString() + "'", 1)[0, 0];
                string[,] branches = oDBEngine.GetFieldValue(" tbl_master_branch ", " branch_id,(branch_description+'['+ltrim(rtrim(branch_code))+']') as branch ", " branch_id in (select cnt_branchid from tbl_master_contact where cnt_internalid='" + Contactid.ToString().Trim() + "')", 2, " branch_description ");
                cls.AddDataToDropDownList(branches, cmbBranch, "All");
                cmbBranch.SelectedIndex = 1;
                string[,] companies = oDBEngine.GetFieldValue(" (select emp_organization from tbl_trans_employeectc where emp_cntid='" + Contactid.ToString() + "' and (emp_effectiveuntil is null or emp_effectiveuntil='1900-01-01 00:00:00.000')) as a inner join tbl_master_company as com on com.cmp_id=a.emp_organization ", " com.cmp_id,com.cmp_name ", null, 2);
                cls.AddDataToDropDownList(companies, cmbCompany, "All");
                cmbCompany.SelectedIndex = 1;
                cmbBranch.Enabled = false;
                cmbCompany.Enabled = false;
                //txtName.Attributes.Add("onkeyup", "CallAjax(this,'employeeBranchHrchy',event)");

                populateCmbYm();

            }

            else

                populatemainGrid();
            //________This script is for firing javascript when page load first___//
            if (!ClientScript.IsStartupScriptRegistered("Today"))
                ClientScript.RegisterStartupScript(typeof(Page), "Today", "<script>PageLoad();</script>");
            //______________________________End Script____________________________//
        }
        protected void populateCmbYm()
        {
            int year = oDBEngine.GetDate().Year;
            cmbYear.Items.Add(year.ToString(), year);
            for (int i = 0; i <= 3; i++)
            {
                year = year - 1;
                cmbYear.Items.Add(year.ToString(), year);
            }
            cmbYear.SelectedIndex = 0;
            cmbYear.SelectedItem.Text = oDBEngine.GetDate().Year.ToString();
            int Imonth = oDBEngine.GetDate().Month;

            cmbMonth.SelectedIndex = 0;
            cmbMonth.SelectedIndex = Imonth - 1;

        }
        protected void populatemainGrid()
        {
            string Contactid = oDBEngine.GetFieldValue("tbl_master_user", "user_contactid", "user_id='" + Session["userid"].ToString() + "'", 1)[0, 0];
            string aa = cmbCompany.SelectedItem.Value.ToString();
            string clasWhere = "emp.emp_contactId = cont.cnt_internalId and ctc.emp_cntId = emp.emp_contactId and comp.cmp_id = ctc.emp_Organization and branch.branch_id = cont.cnt_branchid and atd.atd_cntId = emp.emp_contactId and (emp.emp_dateOfLeaving is null OR emp.emp_dateOfLeaving='1/1/1900 12:00:00 AM' OR emp.emp_dateOfLeaving='1/1/1900' OR (month(emp.emp_dateOfLeaving) >='" + cmbMonth.Value.ToString().Trim() + "' and year(emp.emp_dateOfLeaving) >='" + cmbYear.Value.ToString().Trim() + "'))and atd.atd_year ='" + cmbYear.Value.ToString().Trim() + "' and month(emp.emp_dateofjoining)<=(case when year(emp.emp_dateofjoining)<" + cmbYear.Value.ToString().Trim() + " then 12 when year(emp.emp_dateofjoining)=" + cmbYear.Value.ToString() + " then month(emp.emp_dateofjoining) else '' end)";

            if (cmbYear.SelectedItem.Value.ToString() != "")
            {
                clasWhere = clasWhere + " and atd.atd_year =" + cmbYear.SelectedItem.Value.ToString();
            }
            if (cmbMonth.SelectedItem.Value.ToString() != "")
            {
                clasWhere = clasWhere + " and atd.atd_Month =" + cmbMonth.SelectedItem.Value.ToString();
            }
            if (cmbBranch.SelectedItem.Value.ToString() != "All")
            {
                clasWhere = clasWhere + " and cont.cnt_branchid =" + cmbBranch.SelectedItem.Value.ToString();
            }
            if (cmbCompany.SelectedItem.Value.ToString() != "All")
            {
                clasWhere = clasWhere + " and comp.cmp_id =" + cmbCompany.SelectedItem.Value.ToString();
            }
            if (Contactid.ToString() != "")
            {
                clasWhere = clasWhere + " and cont.cnt_internalId = '" + Contactid.ToString().Trim() + "'";
            }
            /*if (drpEmployee.SelectedValue != "0")
            {
                clasWhere = clasWhere + " and emp.emp_contactId = '" + drpEmployee.SelectedValue + "'";
            }*/
            SDSAttdMain.SelectCommand = "select distinct atd_id, atd_cntId, atd_year, atd_Month,(Isnull(cont.cnt_firstName,'') +''+IsNull(cont.cnt_middleName,'') + ''+Isnull(cont.cnt_lastName,'')) as empName,isnull(cont.cnt_shortName,'') as code,comp.cmp_Name,branch.branch_description , " +
                                        "IsNull(atd_StatusDay1,'N/A') day1, IsNull(atd_StatusDay2,'N/A') day2, IsNull(atd_StatusDay3,'N/A') day3, " +
                                        "IsNull(atd_StatusDay4,'N/A') day4, IsNull(atd_StatusDay5,'N/A') day5, IsNull(atd_StatusDay6,'N/A') day6, " +
                                        "IsNull(atd_StatusDay7,'N/A') day7, IsNull(atd_StatusDay8,'N/A') day8, IsNull(atd_StatusDay9,'N/A') day9, IsNull(atd_StatusDay10,'N/A') day10,  " +
                                        "IsNull(atd_StatusDay11,'N/A') day11, IsNull(atd_StatusDay12,'N/A') day12, IsNull(atd_StatusDay13,'N/A') day13, " +
                                        "IsNull(atd_StatusDay14,'N/A') day14, IsNull(atd_StatusDay15,'N/A') day15, " +
                                        "IsNull(atd_StatusDay16,'N/A') day16, IsNull(atd_StatusDay17,'N/A') day17, " +
                                        "IsNull(atd_StatusDay18,'N/A') day18, IsNull(atd_StatusDay19,'N/A') day19, IsNull(atd_StatusDay20,'N/A') day20, " +
                                        "IsNull(atd_StatusDay21,'N/A') day21, IsNull(atd_StatusDay22,'N/A') day22, IsNull(atd_StatusDay23,'N/A') day23, IsNull(atd_StatusDay24,'N/A') day24, IsNull(atd_StatusDay25,'N/A') day25, IsNull(atd_StatusDay26,'N/A') day26, IsNull(atd_StatusDay27,'N/A') day27, IsNull(atd_StatusDay28,'N/A') day28, IsNull(atd_StatusDay29,'N/A') day29, IsNull(atd_StatusDay30,'N/A') day30, IsNull(atd_StatusDay31,'N/A') day31 " +
                                        "from tbl_master_contact cont,tbl_master_employee emp,tbl_trans_employeeCTC ctc,tbl_master_company comp,tbl_master_branch branch,tbl_trans_attendance atd where " + clasWhere;


            ASPx_EmployeeAtdd.DataBind();
            //if (rbUser.SelectedItem.Value.ToString() == "A")
            //{
            //    Page.ClientScript.RegisterStartupScript(GetType(), "Today", "<script>ShowEmployeeFilterForm('A')</script>");
            //}
            //else
            //{
            //    Page.ClientScript.RegisterStartupScript(GetType(), "Today", "<script>ShowEmployeeFilterForm('S')</script>");
            //}
        }
        protected void ASPx_EmployeeAtdd_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        {
            populatemainGrid();
        }
        protected void changeColor(string value, System.Web.UI.WebControls.TableCell tc)
        {

            if (value == "P")
            {
                tc.BackColor = System.Drawing.Color.FromName("#00B050");
                tc.Text = "";
                tc.ToolTip = "Present!";
            }
            else if (value == "A")
            {
                tc.BackColor = System.Drawing.Color.FromName("#FF0000");
                tc.Text = "";
                tc.ToolTip = "Absent or Leave without Pay!";
            }
            else if (value == "OD")
            {
                tc.BackColor = System.Drawing.Color.FromName("#92D050");
                tc.Text = "";
                tc.ToolTip = "Official Delay!";
            }
            else if (value == "PD")
            {
                tc.BackColor = System.Drawing.Color.FromName("#F87156");
                tc.Text = "";
                tc.ToolTip = "Personal Delay!";
            }
            else if (value == "HC")
            {
                tc.BackColor = System.Drawing.Color.FromName("#C2D69A");
                tc.Text = "";
                tc.ToolTip = "Half day(Casual)!";
            }
            else if (value == "HS")
            {
                tc.BackColor = System.Drawing.Color.FromName("#D7E4BC");
                tc.Text = "";
                tc.ToolTip = "Half day(Sick)!";
            }
            else if (value == "PL")
            {
                tc.BackColor = System.Drawing.Color.FromName("#538ED5");
                tc.Text = "";
                tc.ToolTip = "Privilege Leave!";
            }
            else if (value == "CL" || value == "Cl")
            {
                tc.BackColor = System.Drawing.Color.FromName("#8DB4E3");
                tc.Text = "";
                tc.ToolTip = "Casual Leave!";
            }
            else if (value == "SL")
            {
                tc.BackColor = System.Drawing.Color.FromName("#17375D");
                tc.Text = "";
                tc.ToolTip = "Sick Leave!";
            }
            else if (value == "PH")
            {
                tc.BackColor = System.Drawing.Color.FromName("#E46D0A");
                tc.Text = "";
                tc.ToolTip = "Paid holiday!";
            }
            else if (value == "WO")
            {
                tc.BackColor = System.Drawing.Color.FromName("#953735");
                tc.Text = "";
                tc.ToolTip = "weekly Off!";
            }
            else if (value == "OV")
            {
                tc.BackColor = System.Drawing.Color.FromName("#AAFFAA");
                tc.Text = "";
                tc.ToolTip = "Official Visit!";
            }
            else if (value == "CO")
            {
                tc.BackColor = System.Drawing.Color.FromName("#BC4914");
                tc.Text = "";
                tc.ToolTip = "Compensatory off!";
            }
            else if (value == "N/A")
            {
                tc.BackColor = System.Drawing.Color.GhostWhite;
                tc.Text = "";
                tc.ToolTip = "Attendance Not Present!";
            }
            else
            {
                tc.BackColor = System.Drawing.Color.GhostWhite;
                tc.Text = "";
                tc.ToolTip = "Unrecognized Attendance!";
            }
        }
        protected void ASPx_EmployeeAtdd_HtmlDataCellPrepared(object sender, DevExpress.Web.ASPxGridViewTableDataCellEventArgs e)
        {
            string val = "";
            string aa = e.DataColumn.Caption.ToString();

            if (aa == "1")
            {
                val = e.CellValue.ToString();
                changeColor(val.Trim(), e.Cell);
            }
            else if (aa == "2")
            {
                val = e.CellValue.ToString();
                changeColor(val.Trim(), e.Cell);
            }
            else if (aa == "3")
            {
                val = e.CellValue.ToString();
                changeColor(val.Trim(), e.Cell);
            }
            else if (aa == "4")
            {
                val = e.CellValue.ToString();
                changeColor(val.Trim(), e.Cell);
            }
            else if (aa == "5")
            {
                val = e.CellValue.ToString();
                changeColor(val.Trim(), e.Cell);
            }
            else if (aa == "6")
            {
                val = e.CellValue.ToString();
                changeColor(val.Trim(), e.Cell);
            }
            else if (aa == "7")
            {
                val = e.CellValue.ToString();
                changeColor(val.Trim(), e.Cell);
            }
            else if (aa == "8")
            {
                val = e.CellValue.ToString();
                changeColor(val.Trim(), e.Cell);
            }
            else if (aa == "9")
            {
                val = e.CellValue.ToString();
                changeColor(val.Trim(), e.Cell);
            }
            else if (aa == "10")
            {
                val = e.CellValue.ToString();
                changeColor(val.Trim(), e.Cell);
            }
            else if (aa == "11")
            {
                val = e.CellValue.ToString();
                changeColor(val.Trim(), e.Cell);
            }
            else if (aa == "12")
            {
                val = e.CellValue.ToString();
                changeColor(val.Trim(), e.Cell);
            }
            else if (aa == "13")
            {
                val = e.CellValue.ToString();
                changeColor(val.Trim(), e.Cell);
            }
            else if (aa == "14")
            {
                val = e.CellValue.ToString();
                changeColor(val.Trim(), e.Cell);
            }
            else if (aa == "15")
            {
                val = e.CellValue.ToString();
                changeColor(val.Trim(), e.Cell);
            }
            else if (aa == "16")
            {
                val = e.CellValue.ToString();
                changeColor(val.Trim(), e.Cell);
            }
            else if (aa == "17")
            {
                val = e.CellValue.ToString();
                changeColor(val.Trim(), e.Cell);
            }
            else if (aa == "18")
            {
                val = e.CellValue.ToString();
                changeColor(val.Trim(), e.Cell);
            }
            else if (aa == "19")
            {
                val = e.CellValue.ToString();
                changeColor(val.Trim(), e.Cell);
            }
            else if (aa == "20")
            {
                val = e.CellValue.ToString();
                changeColor(val.Trim(), e.Cell);
            }
            else if (aa == "21")
            {
                val = e.CellValue.ToString();
                changeColor(val.Trim(), e.Cell);
            }
            else if (aa == "22")
            {
                val = e.CellValue.ToString();
                changeColor(val.Trim(), e.Cell);
            }
            else if (aa == "23")
            {
                val = e.CellValue.ToString();
                changeColor(val.Trim(), e.Cell);
            }
            else if (aa == "24")
            {
                val = e.CellValue.ToString();
                changeColor(val.Trim(), e.Cell);
            }
            else if (aa == "25")
            {
                val = e.CellValue.ToString();
                changeColor(val.Trim(), e.Cell);
            }
            else if (aa == "26")
            {
                val = e.CellValue.ToString();
                changeColor(val.Trim(), e.Cell);
            }
            else if (aa == "27")
            {
                val = e.CellValue.ToString();
                changeColor(val.Trim(), e.Cell);
            }
            else if (aa == "28")
            {
                val = e.CellValue.ToString();
                changeColor(val.Trim(), e.Cell);
            }
            else if (aa == "29")
            {
                val = e.CellValue.ToString();
                changeColor(val.Trim(), e.Cell);
            }
            else if (aa == "30")
            {
                val = e.CellValue.ToString();
                changeColor(val.Trim(), e.Cell);
            }
            else if (aa == "31")
            {
                val = e.CellValue.ToString();
                changeColor(val.Trim(), e.Cell);
            }

        }

        protected void ASPx_EmployeeAtdd_CustomJSProperties(object sender, DevExpress.Web.ASPxGridViewClientJSPropertiesEventArgs e)
        {
            e.Properties["cpHeight"] = "a";
        }
    }

}