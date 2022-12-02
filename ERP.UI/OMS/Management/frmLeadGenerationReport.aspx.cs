using System;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrystalDecisions.CrystalReports.Engine;
using BusinessLogicLayer;
using ClsDropDownlistNameSpace;

namespace ERP.OMS.Management
{
    public partial class management_frmLeadGenerationReport : System.Web.UI.Page
    {
        clsDropDownList cls = new clsDropDownList();
        BusinessLogicLayer.Converter OConvert = new BusinessLogicLayer.Converter();
        public string pageAccess = "";
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
               //Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }
            if (!IsPostBack)
            {
                Session["mode"] = "off";
                ComboBind();
                txtFromDate.EditFormatString = OConvert.GetDateFormat("Date");
                txtToDate.EditFormatString = OConvert.GetDateFormat("Date");
                // txtFromDate.Attributes.Add("onfocus", "displayCalendar(txtFromDate ,'dd/mm/yyyy',this,true,null,'0','0')");
                // imgFromDate.Attributes.Add("OnClick", "displayCalendar(txtFromDate ,'dd/mm/yyyy',txtFromDate,true,null,'0','0')");
                //  txtToDate.Attributes.Add("onfocus", "displayCalendar(txtToDate ,'dd/mm/yyyy',this,true,null,'0','0')");
                // imgToDate.Attributes.Add("OnClick", "displayCalendar(txtToDate ,'dd/mm/yyyy',txtToDate,true,null,'0','0')");
            }

            //________This script is for firing javascript when page load ___//
            if (!ClientScript.IsStartupScriptRegistered("Today"))
                ClientScript.RegisterStartupScript(typeof(Page), "Today", "<script>AtTheTimePageLoad();</script>");
            //______________________________End Script____________________________//
            if (Session["mode"] == "on")
            {
                shoReport();
            }
            //this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>height();</script>");
        }

        private void ComboBind()
        {
            //DBEngine oDBEngine = new DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);MULTI
            DBEngine oDBEngine = new DBEngine();
            string[,] Data = oDBEngine.GetFieldValue(" tbl_master_branch ", " branch_id, branch_description", null, 2);
            cls.AddDataToListBox(Data, lst_branch, "All");
            Data = oDBEngine.GetFieldValue(" tbl_master_costCenter ", " cost_id,cost_description", " cost_costCenterType='Department' ", 2);
            cls.AddDataToListBox(Data, lst_department, "All");

            string temp = "";
            string userids = oDBEngine.getChildUserNotColleague(HttpContext.Current.Session["userid"].ToString(), temp);

            Data = oDBEngine.GetFieldValue(" tbl_master_user ", "user_id, user_name", " user_id in (" + userids + ")", 2);
            cls.AddDataToListBox(Data, lst_user, "All");

        }
        protected void btnGetReport_Click(object sender, EventArgs e)
        {
            Session["mode"] = "on";
            Session["KeyVal"] = " ";
            string whereCondition = "";
            BusinessLogicLayer.Converter Oconverter = new BusinessLogicLayer.Converter();
            //string start = Oconverter.DateConverter(txtFromDate.Text, "mm/dd/yyyy");
            string start = txtFromDate.Value.ToString();
            if (txtToDate.Text != "")
            {
                // string end = Oconverter.DateConverter(txtToDate.Text, "mm/dd/yyyy");
                string end = txtToDate.Value.ToString();
                whereCondition = " tbl_master_lead.createDate between '" + start + "' and '" + end + "' ";
            }
            else
                whereCondition = " tbl_master_lead.createDate >= '" + start + "' ";
            if (lst_branch.Items[0].Selected != true)
            {
                string listid = "";
                for (int i = 1; i < lst_branch.Items.Count - 1; i++)
                {
                    if (lst_branch.Items[i].Selected)
                    {
                        if (listid != "")
                            listid += "," + lst_branch.Items[i].Value;
                        else
                            listid = lst_branch.Items[i].Value;
                    }
                }
                whereCondition += " and branch_id IN (" + listid + ") ";
            }
            if (lst_department.Items[0].Selected != true)
            {
                string listid = "";
                for (int i = 1; i < lst_department.Items.Count - 1; i++)
                {
                    if (lst_department.Items[i].Selected)
                    {
                        if (listid != "")
                            listid += "," + lst_department.Items[i].Value;
                        else
                            listid = lst_department.Items[i].Value;
                    }
                }
                //whereCondition += " and emp_Department IN (" + listid + ") ";
            }
            //if (lst_user.Items[0].Selected != true)
            //{
            //    string listid = "";
            //    for (int i = 1; i < lst_user.Items.Count - 1; i++)
            //    {
            //        if (lst_user.Items[i].Selected)
            //        {
            //            if (listid != "")
            //                listid += "," + lst_user.Items[i].Value;
            //            else
            //                listid = lst_user.Items[i].Value;
            //        }
            //    }
            //    whereCondition += " and user_id IN (" + listid + ") ";
            //}
            //else
            //{
            //    string listid = "";
            //    for (int i = 1; i < lst_user.Items.Count - 1; i++)
            //    {
            //        if (listid != "")
            //            listid += "," + lst_user.Items[i].Value;
            //        else
            //            listid = lst_user.Items[i].Value;
            //    }
            //    whereCondition += " and user_id IN (" + listid + ") ";
            //}
            //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);MULTI
            BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
            //DataTable DT = oDBEngine.GetDataTableGroup(" tbl_master_lead INNER JOIN tbl_master_branch ON tbl_master_lead.cnt_branchid = tbl_master_branch.branch_id INNER JOIN tbl_master_user ON tbl_master_lead.CreateUser = tbl_master_user.user_id ", " COUNT(tbl_master_lead.cnt_branchid) AS No, tbl_master_branch.branch_description AS Branch, tbl_master_user.user_name,  tbl_master_user.user_id AS id, tbl_master_lead.CreateUser, (SELECT     TOP 1 tbl_master_costCenter.cost_description FROM  tbl_trans_employeeCTC INNER JOIN tbl_master_costCenter ON tbl_trans_employeeCTC.emp_Department = tbl_master_costCenter.cost_id WHERE(emp_cntId = tbl_master_user.user_contactid) ORDER BY emp_id DESC) AS Department ", whereCondition, " tbl_master_lead.cnt_branchid, tbl_master_branch.branch_description, tbl_master_user.user_name, tbl_master_lead.CreateUser,  tbl_master_user.user_id, tbl_master_user.user_contactId ");
            //gettotal(DT);
            //GridLeadReport.DataSource = DT.DefaultView;
            //GridLeadReport.DataBind();
            DataTable DT = oDBEngine.GetDataTable(" tbl_master_lead INNER JOIN tbl_master_branch ON tbl_master_lead.cnt_branchid = tbl_master_branch.branch_id INNER JOIN tbl_master_user ON tbl_master_lead.CreateUser = tbl_master_user.user_id ", " tbl_master_branch.branch_description AS grp1,(SELECT     TOP 1 tbl_master_costCenter.cost_description FROM          tbl_trans_employeeCTC INNER JOIN tbl_master_costCenter ON tbl_trans_employeeCTC.emp_Department = tbl_master_costCenter.cost_id WHERE(emp_cntId = tbl_master_user.user_contactid) ORDER BY emp_id DESC) AS grp2,tbl_master_user.user_name as grp3, convert(varchar(100),'') as col1, convert(varchar(100),'') as col2, convert(varchar(100),'') as col3, convert(varchar(100),'') as col4, convert(varchar(100),'') as col5, convert(varchar(100),'') as col6 ", whereCondition);
            shoReport();
            ReportDocument LeadReportDocu = new ReportDocument();
            string path = Server.MapPath("..\\Reports\\LeadGenerationReport.rpt");
            LeadReportDocu.Load(path);

            //__________body look__________________//
            Section body = (Section)LeadReportDocu.ReportDefinition.Sections[5];
            body.SectionFormat.EnableSuppress = true;

            TextObject txt1 = (TextObject)LeadReportDocu.ReportDefinition.ReportObjects["Text5"];
            txt1.Text = "";
            TextObject txt2 = (TextObject)LeadReportDocu.ReportDefinition.ReportObjects["Text10"];
            txt2.Text = "";
            TextObject txt3 = (TextObject)LeadReportDocu.ReportDefinition.ReportObjects["Text11"];
            txt3.Text = "";
            TextObject txt4 = (TextObject)LeadReportDocu.ReportDefinition.ReportObjects["Text12"];
            txt4.Text = "";
            TextObject txt5 = (TextObject)LeadReportDocu.ReportDefinition.ReportObjects["Text13"];
            txt5.Text = "";
            TextObject txt6 = (TextObject)LeadReportDocu.ReportDefinition.ReportObjects["Text5"];
            txt6.Text = "";
            TextObject txt7 = (TextObject)LeadReportDocu.ReportDefinition.ReportObjects["Text7"];
            txt7.Text = "Lead Count:";
            TextObject txt8 = (TextObject)LeadReportDocu.ReportDefinition.ReportObjects["Text6"];
            txt8.Text = "Department Count:";

            //__________End________________________//

            LeadReportDocu.SetDataSource(DT);
            CrystalReportViewer1.ReportSource = LeadReportDocu;
            CrystalReportViewer1.DataBind();
        }

        private void shoReport()
        {
            string whereCondition = "";
            //Converter Oconverter = new Converter();
            string start = txtFromDate.Value.ToString();
            if (txtToDate.Text != "")
            {
                string end = txtToDate.Value.ToString();
                whereCondition = " tbl_master_lead.createDate between '" + start + "' and '" + end + "' ";
            }
            else
                whereCondition = " tbl_master_lead.createDate >= '" + start + "' ";
            if (lst_branch.Items[0].Selected != true)
            {
                string listid = "";
                for (int i = 1; i < lst_branch.Items.Count - 1; i++)
                {
                    if (lst_branch.Items[i].Selected)
                    {
                        if (listid != "")
                            listid += "," + lst_branch.Items[i].Value;
                        else
                            listid = lst_branch.Items[i].Value;
                    }
                }
                whereCondition += " and branch_id IN (" + listid + ") ";
            }
            if (lst_department.Items[0].Selected != true)
            {
                string listid = "";
                for (int i = 1; i < lst_department.Items.Count - 1; i++)
                {
                    if (lst_department.Items[i].Selected)
                    {
                        if (listid != "")
                            listid += "," + lst_department.Items[i].Value;
                        else
                            listid = lst_department.Items[i].Value;
                    }
                }
                //whereCondition += " and emp_Department IN (" + listid + ") ";
            }
            //if (lst_user.Items[0].Selected != true)
            //{
            //    string listid = "";
            //    for (int i = 1; i < lst_user.Items.Count - 1; i++)
            //    {
            //        if (lst_user.Items[i].Selected)
            //        {
            //            if (listid != "")
            //                listid += "," + lst_user.Items[i].Value;
            //            else
            //                listid = lst_user.Items[i].Value;
            //        }
            //    }
            //    whereCondition += " and user_id IN (" + listid + ") ";
            //}
            //else
            //{
            //    string listid = "";
            //    for (int i = 1; i < lst_user.Items.Count - 1; i++)
            //    {
            //        if (listid != "")
            //            listid += "," + lst_user.Items[i].Value;
            //        else
            //            listid = lst_user.Items[i].Value;
            //    }
            //    whereCondition += " and user_id IN (" + listid + ") ";
            //}
            //BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);MULTI
            BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
            DataTable DT = oDBEngine.GetDataTable(" tbl_master_lead INNER JOIN tbl_master_branch ON tbl_master_lead.cnt_branchid = tbl_master_branch.branch_id INNER JOIN tbl_master_user ON tbl_master_lead.CreateUser = tbl_master_user.user_id ", " tbl_master_branch.branch_description AS grp1,(SELECT     TOP 1 tbl_master_costCenter.cost_description FROM          tbl_trans_employeeCTC INNER JOIN tbl_master_costCenter ON tbl_trans_employeeCTC.emp_Department = tbl_master_costCenter.cost_id WHERE(emp_cntId = tbl_master_user.user_contactid) ORDER BY emp_id DESC) AS grp2,tbl_master_user.user_name as grp3, convert(varchar(100),'') as col1, convert(varchar(100),'') as col2, convert(varchar(100),'') as col3, convert(varchar(100),'') as col4, convert(varchar(100),'') as col5, convert(varchar(100),'') as col6 ", whereCondition);

            ReportDocument LeadReportDocu = new ReportDocument();
            string path = Server.MapPath("..\\Reports\\LeadGenerationReport.rpt");
            LeadReportDocu.Load(path);

            //__________body look__________________//
            Section body = (Section)LeadReportDocu.ReportDefinition.Sections[5];
            body.SectionFormat.EnableSuppress = true;

            TextObject txt1 = (TextObject)LeadReportDocu.ReportDefinition.ReportObjects["Text5"];
            txt1.Text = "";
            TextObject txt2 = (TextObject)LeadReportDocu.ReportDefinition.ReportObjects["Text10"];
            txt2.Text = "";
            TextObject txt3 = (TextObject)LeadReportDocu.ReportDefinition.ReportObjects["Text11"];
            txt3.Text = "";
            TextObject txt4 = (TextObject)LeadReportDocu.ReportDefinition.ReportObjects["Text12"];
            txt4.Text = "";
            TextObject txt5 = (TextObject)LeadReportDocu.ReportDefinition.ReportObjects["Text13"];
            txt5.Text = "";
            TextObject txt6 = (TextObject)LeadReportDocu.ReportDefinition.ReportObjects["Text5"];
            txt6.Text = "";
            TextObject txt7 = (TextObject)LeadReportDocu.ReportDefinition.ReportObjects["Text7"];
            txt7.Text = "Lead Count:";
            TextObject txt8 = (TextObject)LeadReportDocu.ReportDefinition.ReportObjects["Text6"];
            txt8.Text = "Department Count:";

            //__________End________________________//

            LeadReportDocu.SetDataSource(DT);
            CrystalReportViewer1.ReportSource = LeadReportDocu;
            CrystalReportViewer1.DataBind();
        }
        int Total_No_Of_Lead = 0;
        private void gettotal(DataTable DT)
        {
            for (int i = 0; i < DT.Rows.Count; i++)
            {
                Total_No_Of_Lead += int.Parse(DT.Rows[i]["No"].ToString());
            }
        }
        public int getTotalOfLead()
        {
            return Total_No_Of_Lead;
        }
        string branchname = "";
        string Department = "";

        protected void GridLeadReport_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (branchname == "" || branchname.Trim() != e.Row.Cells[0].Text)
                {
                    Department = "";
                    if (Session["KeyVal"].ToString() == " ")
                    {
                        Session["KeyVal"] = " ";
                        branchname = e.Row.Cells[0].Text;
                    }
                    else
                    {
                        Session["KeyVal"] = " ";
                        branchname = e.Row.Cells[0].Text;
                        e.Row.Cells[0].Text = "";
                    }
                }
                else
                    e.Row.Cells[0].Text = "";
                if (Department == "" || Department.Trim() != e.Row.Cells[1].Text)
                {
                    if (Session["KeyVal"].ToString() == " ")
                    {
                        Session["KeyVal"] = " ";
                        Department = e.Row.Cells[1].Text;
                    }
                    else
                    {
                        Session["KeyVal"] = " ";
                        Department = e.Row.Cells[1].Text;
                        e.Row.Cells[1].Text = "";
                    }
                }
                else
                    e.Row.Cells[1].Text = "";
            }
        }
        protected void txtToDate_DateChanged(object sender, EventArgs e)
        {

        }
    }
}