using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web;
using System.Configuration;
using BusinessLogicLayer;

namespace ERP.OMS.Reports
{
    public partial class Reports_NewfrmperformanceReport_telecaller : System.Web.UI.Page
    {
        DataTable DT_main = new DataTable();
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);
        BusinessLogicLayer.Converter OConvert = new BusinessLogicLayer.Converter();
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
        protected void Page_Load(object sender, EventArgs e)
        {
            if (HttpContext.Current.Session["userid"] == null)
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "SighOff", "<script>SignOff();</script>");
            }

            if (!IsPostBack)
            {

                txtFromDate.EditFormatString = OConvert.GetDateFormat("Date");
                txtToDate.EditFormatString = OConvert.GetDateFormat("Date");
                txtFromDate.Value = oDBEngine.GetDate();
                txtToDate.Value = oDBEngine.GetDate();
                //txtFromDate.Attributes.Add("onfocus", "displayCalendar(ctl00_ContentPlaceHolder3_txtFromDate ,'dd/mm/yyyy',this,true,null,'0','0')");
                //imgFromDate.Attributes.Add("OnClick", "displayCalendar(ctl00_ContentPlaceHolder3_txtFromDate ,'dd/mm/yyyy',ctl00_ContentPlaceHolder3_txtFromDate,true,null,'0','0')");
                //txtToDate.Attributes.Add("onfocus", "displayCalendar(ctl00_ContentPlaceHolder3_txtToDate ,'dd/mm/yyyy',this,true,null,'0','0')");
                //imgToDate.Attributes.Add("OnClick", "displayCalendar(ctl00_ContentPlaceHolder3_txtToDate ,'dd/mm/yyyy',ctl00_ContentPlaceHolder3_txtToDate,true,null,'0','0')");
                //txtFromDate.Attributes.Add("readonly", "true");
                //txtToDate.Attributes.Add("readonly", "true");
                Session["mode"] = "off";
                ASPxComboBox1.Visible = false;
                TLLead.Visible = false;
            }
            if (Session["mode"].ToString() == "on")
                ShowReport();
            //this.Page.ClientScript.RegisterStartupScript(GetType(), "heightL", "<script>height();</script>");
        }

        private void ShowReport()
        {
            Session["mode"] = "on";
            ASPxComboBox1.Visible = true;
            TLLead.Visible = true;
            DT_main = new DataTable();
            if (DT_main.Rows.Count != 0)
            {
                //______Table Design______________///
                DataColumn col1 = new DataColumn("Caller");
                DataColumn col2 = new DataColumn("TotalCalls");
                DataColumn col3 = new DataColumn("CallBack");
                DataColumn col4 = new DataColumn("SalesVisit");
                DataColumn col5 = new DataColumn("ConfirmSales");
                DataColumn col6 = new DataColumn("NotInterested");
                DataColumn col7 = new DataColumn("NonContactable");
                DataColumn col8 = new DataColumn("NonUsabled");
                DataColumn col9 = new DataColumn("Contactiblity");
                DataColumn col10 = new DataColumn("Effectiveness");
                DataColumn col11 = new DataColumn("Avg");
                DataColumn col12 = new DataColumn("User_ID");
                DataColumn col13 = new DataColumn("User_ParentId");

                DT_main.Columns.Add(col1);
                DT_main.Columns.Add(col2);
                DT_main.Columns.Add(col3);
                DT_main.Columns.Add(col4);
                DT_main.Columns.Add(col5);
                DT_main.Columns.Add(col6);
                DT_main.Columns.Add(col7);
                DT_main.Columns.Add(col8);
                DT_main.Columns.Add(col9);
                DT_main.Columns.Add(col10);
                DT_main.Columns.Add(col11);
                DT_main.Columns.Add(col12);
                DT_main.Columns.Add(col13);
                //__________-End Here______//

                //__________calculating Logedin user`s Calls activity___________-//
                CalculateEmployeeCalls(HttpContext.Current.Session["userid"].ToString(), "0");

                //___________Now calling function to Chiled User`s work regarding Calls_____//
                GetUserList_call_CalculateEmployeeCalls(HttpContext.Current.Session["userid"].ToString());

                TLLead.DataSource = DT_main.DefaultView;
                TLLead.DataBind();
            }
            else
            {
                lblError.Text = "No Data found.";
                lblError.Visible = true;
                TLLead.Visible = false;
                tblExportV.Visible = false;
            }
        }

        private void GetUserList_call_CalculateEmployeeCalls(string EmployeeID)
        {
            if (EmployeeID != "")
            {
                string temp = "";

                string ListOfUser = "";
                DataTable DtSecond = oDBEngine.GetDataTable(" tbl_trans_employeeCTC,tbl_master_employee ", " tbl_master_employee.emp_id ", " tbl_trans_employeeCTC.emp_reportTo=" + EmployeeID + " and tbl_master_employee.emp_contactId =tbl_trans_employeeCTC.emp_cntId and tbl_trans_employeeCTC.emp_effectiveuntil is null AND (tbl_master_employee.emp_dateofLeaving IS NULL OR tbl_master_employee.emp_dateofLeaving = '1/1/1900' Or tbl_master_employee.emp_dateofLeaving = '01/01/1900' ) ");

                if (DtSecond.Rows.Count != 0)
                {
                    for (int i = 0; i < DtSecond.Rows.Count; i++)
                    {
                        ListOfUser += DtSecond.Rows[i][0].ToString() + ",";
                    }
                }
                string[] userids = ListOfUser.Split(',');
                for (int i = 0; i < userids.Length; i++)
                {
                    if (userids[i] != "")
                    {
                        CalculateEmployeeCalls(userids[i], EmployeeID);
                        GetUserList_call_CalculateEmployeeCalls(userids[i]);
                    }
                }
            }
        }

        private void CalculateEmployeeCalls(string EmployeeId, string ReportingHeadId)
        {
            if (EmployeeId != "")
            {
                //DBEngine oDBEngine = new DBEngine(ConfigurationSettings.AppSettings["DBConnectionDefault"]);
                string usercondition = "";
                usercondition = " and tbl_trans_phonecalldetails.createuser=" + EmployeeId;
                Converter oConverter = new Converter();
                string startdate = txtFromDate.Date.Month.ToString() + "/" + txtFromDate.Date.Day.ToString() + "/" + txtFromDate.Date.Year.ToString() + " 01:00 AM";
                string Enddate = txtToDate.Date.Month.ToString() + "/" + txtToDate.Date.Day.ToString() + "/" + txtToDate.Date.Year.ToString() + " 11:55 PM";

                int totalNumbercalls = 0;
                int totalNumbercallBack = 0;
                int totalnumbernoncontactable = 0;
                int totalnumbernonusable = 0;
                int totalnumbersalesvisit = 0;
                int totalnumbersales = 0;
                int totalnumberlost = 0;
                int totalnumberMin = 0;

                DataTable DT = oDBEngine.GetDataTable(" tbl_trans_phonecalldetails INNER JOIN tbl_master_calldispositions ON tbl_trans_phonecalldetails.phd_callDispose = tbl_master_calldispositions.call_id INNER JOIN tbl_Master_DispositionCategory ON tbl_master_calldispositions.Call_Category = tbl_Master_DispositionCategory.Int_id  ", " COUNT(tbl_trans_phonecalldetails.phd_id) AS id, tbl_trans_phonecalldetails.CreateUser, tbl_Master_DispositionCategory.Int_id,SUM(tbl_trans_phonecalldetails.phd_callduration) AS Duration, COUNT(DISTINCT tbl_trans_phonecalldetails.phd_phoneCallId) ", " (CAST(tbl_trans_phonecalldetails.phd_callDate AS datetime) >= CONVERT(varchar,'" + startdate + "', 101)) and (CAST(tbl_trans_phonecalldetails.phd_callDate AS datetime) <= CONVERT(varchar,'" + Enddate + "', 101)) AND (tbl_trans_phonecalldetails.phd_CallType IS NULL)" + usercondition, " tbl_trans_phonecalldetails.CreateUser, tbl_Master_DispositionCategory.Int_id ", "tbl_trans_phonecalldetails.createuser");
                if (DT.Rows.Count > 0)
                {
                    DataRow DR = DT_main.NewRow();
                    int userid = 0;
                    int totalcall = 0;
                    int totalmin = 0;
                    int nonusable = 0;
                    int noncontactable = 0;
                    int notinterested = 0;
                    int totalsales = 0;

                    for (int i = 0; i < DT.Rows.Count; i++)
                    {
                        if (userid != int.Parse(DT.Rows[i][1].ToString()))
                        {
                            if (i == 0)
                            {
                                DR = DT_main.NewRow();
                            }
                            else
                            {
                                DR[1] = totalcall;
                                try
                                {
                                    DR[8] = (int)(((totalcall - nonusable - noncontactable) * 100) / totalcall);
                                    DR[9] = (int)((totalsales * 100) / (totalcall - nonusable - noncontactable));
                                }
                                catch (Exception ex)
                                {
                                    DR[8] = 0;
                                    DR[9] = 0;
                                }
                                totalnumberMin += totalmin;
                                int min = (totalmin / 60) / totalcall;
                                DR[10] = min;
                                DR[11] = EmployeeId;
                                DR[12] = ReportingHeadId;

                                DT_main.Rows.Add(DR);

                                DR = DT_main.NewRow();

                                totalsales = 0;
                                totalmin = 0;
                                totalcall = 0;
                                nonusable = 0;
                                noncontactable = 0;
                                notinterested = 0;

                            }

                            userid = int.Parse(DT.Rows[i][1].ToString());
                            DR[0] = oDBEngine.GetFieldValue(" tbl_master_user ", " user_name ", " user_id='" + DT.Rows[0]["CreateUser"].ToString() + "'", 1)[0, 0].ToString();
                            switch (DT.Rows[0][2].ToString())
                            {
                                case "1":
                                    totalNumbercallBack += int.Parse(DT.Rows[i][4].ToString());
                                    DR[2] = DT.Rows[i][4].ToString();
                                    totalcall += int.Parse(DT.Rows[i][0].ToString());
                                    totalmin += int.Parse(DT.Rows[i][3].ToString());
                                    break;
                                case "2":
                                    totalnumbernoncontactable += int.Parse(DT.Rows[i][4].ToString());
                                    DR[6] = int.Parse(DT.Rows[i][4].ToString());
                                    totalcall += int.Parse(DT.Rows[i][0].ToString());
                                    totalmin += int.Parse(DT.Rows[i][3].ToString());
                                    noncontactable += int.Parse(DT.Rows[i][4].ToString());
                                    break;
                                case "3":
                                    totalnumbernonusable += int.Parse(DT.Rows[i][4].ToString());
                                    DR[7] = DT.Rows[i][4].ToString();
                                    totalcall += int.Parse(DT.Rows[i][0].ToString());
                                    totalmin += int.Parse(DT.Rows[i][3].ToString());
                                    nonusable += int.Parse(DT.Rows[i][4].ToString());
                                    break;
                                case "4":
                                    totalnumbersalesvisit += int.Parse(DT.Rows[i][4].ToString());
                                    totalsales += int.Parse(DT.Rows[i][4].ToString());
                                    DR[3] = DT.Rows[i][4].ToString();
                                    totalcall += int.Parse(DT.Rows[i][0].ToString());
                                    totalmin += int.Parse(DT.Rows[i][3].ToString());
                                    break;
                                case "5":
                                    totalnumbersales += int.Parse(DT.Rows[i][4].ToString());
                                    totalsales += int.Parse(DT.Rows[i][4].ToString());
                                    DR[4] = DT.Rows[i][4].ToString();
                                    totalcall += int.Parse(DT.Rows[i][0].ToString());
                                    totalmin += int.Parse(DT.Rows[i][3].ToString());
                                    break;
                                case "6":
                                    totalnumberlost += int.Parse(DT.Rows[i][4].ToString());
                                    DR[5] = DT.Rows[i][4].ToString();
                                    totalcall += int.Parse(DT.Rows[i][0].ToString());
                                    totalmin += int.Parse(DT.Rows[i][3].ToString());
                                    notinterested += int.Parse(DT.Rows[i][4].ToString());
                                    break;

                            }
                        }
                        switch (DT.Rows[0][2].ToString())
                        {
                            case "1":
                                totalNumbercallBack += int.Parse(DT.Rows[i][4].ToString());
                                DR[2] = DT.Rows[i][4].ToString();
                                totalcall += int.Parse(DT.Rows[i][0].ToString());
                                totalmin += int.Parse(DT.Rows[i][3].ToString());
                                break;
                            case "2":
                                totalnumbernoncontactable += int.Parse(DT.Rows[i][4].ToString());
                                DR[6] = int.Parse(DT.Rows[i][4].ToString());
                                totalcall += int.Parse(DT.Rows[i][0].ToString());
                                totalmin += int.Parse(DT.Rows[i][3].ToString());
                                noncontactable += int.Parse(DT.Rows[i][4].ToString());
                                break;
                            case "3":
                                totalnumbernonusable += int.Parse(DT.Rows[i][4].ToString());
                                DR[7] = DT.Rows[i][4].ToString();
                                totalcall += int.Parse(DT.Rows[i][0].ToString());
                                totalmin += int.Parse(DT.Rows[i][3].ToString());
                                nonusable += int.Parse(DT.Rows[i][4].ToString());
                                break;
                            case "4":
                                totalnumbersalesvisit += int.Parse(DT.Rows[i][4].ToString());
                                totalsales += int.Parse(DT.Rows[i][4].ToString());
                                DR[3] = DT.Rows[i][4].ToString();
                                totalcall += int.Parse(DT.Rows[i][0].ToString());
                                totalmin += int.Parse(DT.Rows[i][3].ToString());
                                break;
                            case "5":
                                totalnumbersales += int.Parse(DT.Rows[i][4].ToString());
                                totalsales += int.Parse(DT.Rows[i][4].ToString());
                                DR[4] = DT.Rows[i][4].ToString();
                                totalcall += int.Parse(DT.Rows[i][0].ToString());
                                totalmin += int.Parse(DT.Rows[i][3].ToString());
                                break;
                            case "6":
                                totalnumberlost += int.Parse(DT.Rows[i][4].ToString());
                                DR[5] = DT.Rows[i][4].ToString();
                                totalcall += int.Parse(DT.Rows[i][0].ToString());
                                totalmin += int.Parse(DT.Rows[i][3].ToString());
                                notinterested += int.Parse(DT.Rows[i][4].ToString());
                                break;

                        }
                        if (i == DT.Rows.Count - 1)
                        {
                            DR[1] = totalcall;
                            try
                            {
                                DR[8] = (int)(((totalcall - nonusable - noncontactable) * 100) / totalcall);
                                DR[9] = (int)((totalsales * 100) / (totalcall - nonusable - noncontactable));
                            }
                            catch (Exception ex)
                            {
                                DR[8] = 0;
                                DR[9] = 0;
                            }
                            totalnumberMin += totalmin;
                            int min = (totalmin / 60) / totalcall;
                            DR[10] = min;
                            DR[11] = EmployeeId;
                            DR[12] = ReportingHeadId;

                            DT_main.Rows.Add(DR);
                        }
                    }

                }
            }
        }

        protected void btnGetReport_Click(object sender, EventArgs e)
        {
            ShowReport();
        }
        protected void ASPxComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ASPxComboBox1.Value.ToString() != "")
            {
                ASPxTreeListExporter1.Settings.PageSettings.Landscape = true;
                ASPxTreeListExporter1.Settings.PageSettings.Margins.Left = 2;
                ASPxTreeListExporter1.Settings.PageSettings.Margins.Right = 2;
                ASPxTreeListExporter1.TreeList.Font.Size = FontUnit.Parse("10px");
                if (ASPxComboBox1.Value.ToString() == "Pdf")
                    ASPxTreeListExporter1.WritePdfToResponse();
                if (ASPxComboBox1.Value.ToString() == "Xls")
                    ASPxTreeListExporter1.WriteXlsToResponse();
                if (ASPxComboBox1.Value.ToString() == "Rtf")
                    ASPxTreeListExporter1.WriteRtfToResponse();
            }
        }
        protected void TLLead_HtmlRowPrepared(object sender, DevExpress.Web.ASPxTreeList.TreeListHtmlRowEventArgs e)
        {
            if (e.RowKind == DevExpress.Web.ASPxTreeList.TreeListRowKind.GroupFooter)
            {
                ASPxLabel lblcontactblt = TLLead.FindGroupFooterTemplateControl(e.NodeKey, TLLead.Columns["Contactiblity"], "ASPxLabel1") as ASPxLabel;
                //ASPxLabel lblConfSales = TLLead.FindGroupFooterTemplateControl(e.NodeKey, TLLead.Columns["ConfirmSales"], "ASPxLabel1") as ASPxLabel;
                lblcontactblt.Text = ((int.Parse(TLLead.FindNodeByKeyValue(e.NodeKey)["SalesVisit"].ToString()) + int.Parse(TLLead.FindNodeByKeyValue(e.NodeKey)["ConfirmSales"].ToString())) / (int.Parse(TLLead.FindNodeByKeyValue(e.NodeKey)["TotalCalls"].ToString()) - int.Parse(TLLead.FindNodeByKeyValue(e.NodeKey)["NonUsabled"].ToString()) - int.Parse(TLLead.FindNodeByKeyValue(e.NodeKey)["NonContactable"].ToString())) * 100) + "%";

            }
            if (e.RowKind == DevExpress.Web.ASPxTreeList.TreeListRowKind.Footer)
            {
                //ASPxLabel lblcontactblt = TLLead.FindGroupFooterTemplateControl(e.NodeKey, TLLead.Columns["Contactiblity"], "ASPxLabel1") as ASPxLabel;
                int ttt = int.Parse(TLLead.FindNodeByKeyValue(e.NodeKey)["SalesVisit"].ToString());
                //lblcontactblt.Text = ((int.Parse(TLLead.FindNodeByKeyValue(e.NodeKey)["SalesVisit"].ToString()) + int.Parse(TLLead.FindNodeByKeyValue(e.NodeKey)["ConfirmSales"].ToString())) / (int.Parse(TLLead.FindNodeByKeyValue(e.NodeKey)["TotalCalls"].ToString()) - int.Parse(TLLead.FindNodeByKeyValue(e.NodeKey)["NonUsabled"].ToString()) - int.Parse(TLLead.FindNodeByKeyValue(e.NodeKey)["NonContactable"].ToString())) * 100) + "%";
            }
        }

    }
}