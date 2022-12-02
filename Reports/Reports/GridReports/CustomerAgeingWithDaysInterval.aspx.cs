using DevExpress.Web;
using DevExpress.Web.Mvc;
using EntityLayer.CommonELS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogicLayer;
using System.Collections.Specialized;
using System.Collections;
using System.Text;
using System.Data.SqlClient;
using System.Configuration;
using System.Threading.Tasks;
using DevExpress.XtraPrinting;
using DevExpress.Export;
using System.Drawing;
using Reports.Model; 

namespace Reports.Reports.GridReports
{
    public partial class CustomerAgeingWithDaysInterval : System.Web.UI.Page
    {
        //DataTable DTIndustry = new DataTable();
        BusinessLogicLayer.Reports objReport = new BusinessLogicLayer.Reports();
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();

        //DataTable dtPartyTotal = null;

        protected void Page_PreInit(object sender, EventArgs e) // lead add
        {
            if (!IsPostBack)
            {
                string sPath = Convert.ToString(HttpContext.Current.Request.Url);
                oDBEngine.Call_CheckPageaccessebility(sPath);
            }
        }
        protected void Page_Init(object sender, EventArgs e) // lead add
        {
            rights = new UserRightsForPage();
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/GridReports/CustomerAgeingWithDaysInterval.aspx");
            if (!IsPostBack)
            {
                if (ShowGridCustAgeingWithDaysInterval.Columns["DOC_NO"] == null)
                {
                    //==============By default Grid populate in load===================
                   


                    GridViewDataTextColumn col0 = new GridViewDataTextColumn();
                    col0.FieldName = "PARTYNAME";
                    col0.Caption = "Customer Name";
                    col0.CellStyle.HorizontalAlign = HorizontalAlign.Left;
                    col0.Width = 350;
                    col0.GroupIndex = 1;

                    ShowGridCustAgeingWithDaysInterval.Columns.Add(col0);

                    GridViewDataTextColumn col1 = new GridViewDataTextColumn();
                    col1.FieldName = "BRANCH_DESCRIPTION";
                    col1.Caption = "Unit";
                    col1.CellStyle.HorizontalAlign = HorizontalAlign.Left;
                    col1.Width = 350;
                    ShowGridCustAgeingWithDaysInterval.Columns.Add(col1);

                    GridViewDataTextColumn col2 = new GridViewDataTextColumn();
                    col2.FieldName = "DOC_NO";
                    col2.Caption = "Doc. No";
                    col2.CellStyle.HorizontalAlign = HorizontalAlign.Left;
                    col2.Width = 200;
                    ShowGridCustAgeingWithDaysInterval.Columns.Add(col2);

                    GridViewDataTextColumn col3 = new GridViewDataTextColumn();
                    col3.FieldName = "DOC_TYPE";
                    col3.Caption = "Doc. Type";
                    col3.CellStyle.HorizontalAlign = HorizontalAlign.Left;
                    col3.Width = 200;
                    ShowGridCustAgeingWithDaysInterval.Columns.Add(col3);

                    GridViewDataTextColumn col4 = new GridViewDataTextColumn();
                    col4.FieldName = "ISOPENING";
                    col4.Caption = "Opening?";
                    col4.CellStyle.HorizontalAlign = HorizontalAlign.Left;
                    col4.Width = 150;
                    ShowGridCustAgeingWithDaysInterval.Columns.Add(col4);

                    GridViewDataTextColumn col5 = new GridViewDataTextColumn();
                    col5.FieldName = "DOC_DATE";
                    col5.Caption = "Doc. Date";
                    col5.CellStyle.HorizontalAlign = HorizontalAlign.Left;
                    col5.Width = 150;
                    ShowGridCustAgeingWithDaysInterval.Columns.Add(col5);

                    GridViewDataTextColumn col6 = new GridViewDataTextColumn();
                    col6.FieldName = "DUE_DATE";
                    col6.Caption = "Due Date";
                    col6.CellStyle.HorizontalAlign = HorizontalAlign.Left;
                    col6.Width = 150;
                    ShowGridCustAgeingWithDaysInterval.Columns.Add(col6);

                    GridViewDataTextColumn col7 = new GridViewDataTextColumn();
                    col7.FieldName = "DOC_AMOUNT";
                    col7.Caption = "Doc. Amt.";
                    col7.CellStyle.HorizontalAlign = HorizontalAlign.Right;
                    col7.Width = 150;
                    col7.HeaderStyle.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Right;
                    //Rev Debashis Refer:0019573
                    col7.PropertiesTextEdit.DisplayFormatString = "0.00";
                    //End of Rev Debashis
                    ShowGridCustAgeingWithDaysInterval.Columns.Add(col7);

                    GridViewDataTextColumn col8 = new GridViewDataTextColumn();
                    col8.FieldName = "CUMBAL_AMOUNT";
                    col8.Caption = "Balance";
                    col8.CellStyle.HorizontalAlign = HorizontalAlign.Right;
                    col8.Width = 150;
                    col8.HeaderStyle.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Right;
                    //Rev Debashis Refer:0019573
                    col8.PropertiesTextEdit.DisplayFormatString = "0.00";
                    //End of Rev Debashis
                    ShowGridCustAgeingWithDaysInterval.Columns.Add(col8);
                 
                    //=====================================================================

                }
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            DateTime dtFrom;
            DateTime dtTo;
            //ShowGridCustAgeingWithDaysInterval.Columns.Clear();
            if (!IsPostBack)
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("PARTYNAME");
                dt.Columns.Add("BRANCH_DESCRIPTION");
                dt.Columns.Add("DOC_NO");
                dt.Columns.Add("DOC_TYPE");
                dt.Columns.Add("ISOPENING");
                dt.Columns.Add("DOC_DATE");
                dt.Columns.Add("DUE_DATE");
                dt.Columns.Add("DOC_AMOUNT");
                dt.Columns.Add("CUMBAL_AMOUNT");


                DataTable dtBranchSelection = new DataTable();
                string GridHeader = "";
                BusinessLogicLayer.Reports GridHeaderDet = new BusinessLogicLayer.Reports();
                RptHeading.Text = "Customer Ageing With Days Interval";
                GridHeader = GridHeaderDet.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), true, false, false, false, false, false);
                CompName.Text = GridHeader;

                GridHeader = GridHeaderDet.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), false, true, false, false, false, false);
                CompAdd.Text = GridHeader;

                GridHeader = GridHeaderDet.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), false, false, true, false, false, false);
                CompOth.Text = GridHeader;

                GridHeader = GridHeaderDet.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), false, false, false, true, false, false);
                CompPh.Text = GridHeader;

                GridHeader = GridHeaderDet.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), false, false, false, false, false, true);
                CompAccPrd.Text = GridHeader;

                Session["SI_ComponentData_Branch"] = null;
                BranchHoOffice();
                Session["exportval"] = null;
                Session["CustAgeingWithDaysIntrvl"] = null;
                Session["CustAgeingTotal"] = null;
                Session["Intervl"] = null;
                Session["HeadersAmountCaption"] = null;
                Session["HeadersAmountFields"] = null;

                Session["chk_presenttotal"] = 0;

                dtFrom = DateTime.Now;
                dtTo = DateTime.Now;
                ASPxAsOnDate.Value = DateTime.Now;
                Date_finyearwise(Convert.ToString(Session["LastFinYear"]));
                //chkallcust.Attributes.Add("OnClick", "CustAll('allcust')");
                //Rev Subhra 24-12-2018   0017670
                Session["BranchNames"] = null;
                //End Rev
                dtBranchSelection = oDBEngine.GetDataTable("select Variable_Value from Config_SystemSettings where Variable_Name='ReportsBranchSelection'");
                hdnBranchSelection.Value = dtBranchSelection.Rows[0][0].ToString();

                ShowGridCustAgeingWithDaysInterval.DataSource = dt;
                ShowGridCustAgeingWithDaysInterval.DataBind();
            }
            else
            {
                dtFrom = Convert.ToDateTime(ASPxAsOnDate.Date);

                if (ShowGridCustAgeingWithDaysInterval.Columns["DOC_NO"] == null)
                {
                    //==============By default Grid populate in load===================



                    GridViewDataTextColumn col0 = new GridViewDataTextColumn();
                    col0.FieldName = "PARTYNAME";
                    col0.Caption = "Customer Name";
                    col0.CellStyle.HorizontalAlign = HorizontalAlign.Left;
                    col0.Width = 350;
                    col0.GroupIndex = 1;

                    ShowGridCustAgeingWithDaysInterval.Columns.Add(col0);

                    GridViewDataTextColumn col1 = new GridViewDataTextColumn();
                    col1.FieldName = "BRANCH_DESCRIPTION";
                    col1.Caption = "Unit";
                    col1.CellStyle.HorizontalAlign = HorizontalAlign.Left;
                    col1.Width = 350;
                    ShowGridCustAgeingWithDaysInterval.Columns.Add(col1);

                    GridViewDataTextColumn col2 = new GridViewDataTextColumn();
                    col2.FieldName = "DOC_NO";
                    col2.Caption = "Doc. No";
                    col2.CellStyle.HorizontalAlign = HorizontalAlign.Left;
                    col2.Width = 200;
                    ShowGridCustAgeingWithDaysInterval.Columns.Add(col2);

                    GridViewDataTextColumn col3 = new GridViewDataTextColumn();
                    col3.FieldName = "DOC_TYPE";
                    col3.Caption = "Doc. Type";
                    col3.CellStyle.HorizontalAlign = HorizontalAlign.Left;
                    col3.Width = 200;
                    ShowGridCustAgeingWithDaysInterval.Columns.Add(col3);

                    GridViewDataTextColumn col4 = new GridViewDataTextColumn();
                    col4.FieldName = "ISOPENING";
                    col4.Caption = "Opening?";
                    col4.CellStyle.HorizontalAlign = HorizontalAlign.Left;
                    col4.Width = 150;
                    ShowGridCustAgeingWithDaysInterval.Columns.Add(col4);

                    GridViewDataTextColumn col5 = new GridViewDataTextColumn();
                    col5.FieldName = "DOC_DATE";
                    col5.Caption = "Doc. Date";
                    col5.CellStyle.HorizontalAlign = HorizontalAlign.Left;
                    col5.Width = 150;
                    ShowGridCustAgeingWithDaysInterval.Columns.Add(col5);

                    GridViewDataTextColumn col6 = new GridViewDataTextColumn();
                    col6.FieldName = "DUE_DATE";
                    col6.Caption = "Due Date";
                    col6.CellStyle.HorizontalAlign = HorizontalAlign.Left;
                    col6.Width = 150;
                    ShowGridCustAgeingWithDaysInterval.Columns.Add(col6);

                    GridViewDataTextColumn col7 = new GridViewDataTextColumn();
                    col7.FieldName = "DOC_AMOUNT";
                    col7.Caption = "Doc. Amt.";
                    col7.CellStyle.HorizontalAlign = HorizontalAlign.Right;
                    col7.Width = 150;
                    col7.HeaderStyle.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Right;
                    //Rev Debashis Refer:0019573
                    col7.PropertiesTextEdit.DisplayFormatString = "0.00";
                    //End of Rev Debashis
                    ShowGridCustAgeingWithDaysInterval.Columns.Add(col7);

                    GridViewDataTextColumn col8 = new GridViewDataTextColumn();
                    col8.FieldName = "CUMBAL_AMOUNT";
                    col8.Caption = "Balance";
                    col8.CellStyle.HorizontalAlign = HorizontalAlign.Right;
                    col8.Width = 150;
                    col8.HeaderStyle.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Right;
                    //Rev Debashis Refer:0019573
                    col8.PropertiesTextEdit.DisplayFormatString = "0.00";
                    //End of Rev Debashis
                    ShowGridCustAgeingWithDaysInterval.Columns.Add(col8);

                    //=====================================================================

                }
            }
            //if (!IsPostBack)
            //{
                
            //}
            if (!IsPostBack)
            {
                string COMPANYID = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
                string Finyear = Convert.ToString(HttpContext.Current.Session["LastFinYear"]);
                dtFrom = Convert.ToDateTime(ASPxAsOnDate.Date);

                string ASONDATE = dtFrom.ToString("yyyy-MM-dd");
                string BRANCH_ID = "";
                if (hdnSelectedBranches.Value != "")
                {
                    BRANCH_ID = hdnSelectedBranches.Value;
                }

            }
        }

        public void Date_finyearwise(string Finyear)
        {
            CommonBL cbl = new CommonBL();
            DataTable tcbl = new DataTable();

            tcbl = cbl.GetDateFinancila(Finyear);
            if (tcbl.Rows.Count > 0)
            {
                ASPxAsOnDate.MaxDate = Convert.ToDateTime((tcbl.Rows[0]["FinYear_EndDate"]));
                ASPxAsOnDate.MinDate = Convert.ToDateTime((tcbl.Rows[0]["FinYear_StartDate"]));

                DateTime MaximumDate = Convert.ToDateTime((tcbl.Rows[0]["FinYear_EndDate"]));
                DateTime MinimumDate = Convert.ToDateTime((tcbl.Rows[0]["FinYear_StartDate"]));

                DateTime TodayDate = Convert.ToDateTime(DateTime.Now);
                DateTime FinYearEndDate = MaximumDate;

                if (TodayDate > FinYearEndDate)
                {
                    ASPxAsOnDate.Date = FinYearEndDate;
                }
                else
                {
                    ASPxAsOnDate.Date = TodayDate;
                }
            }

        }
        #region Export


        public void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 Filter = int.Parse(Convert.ToString(drdExport.SelectedItem.Value));
            if (Filter != 0)
            {

                BranchHoOffice();
                if (Session["exportval"] == null)
                {
                    Session["exportval"] = Filter;
                    bindexport(Filter);
                }
                else if (Convert.ToInt32(Session["exportval"]) != Filter)
                {
                    Session["exportval"] = Filter;
                    bindexport(Filter);
                }
            }
        }
        public void bindexport(int Filter)
        {

            string filename = "Customer Ageing With Days Interval";
            exporter.FileName = filename;
            string FileHeader = "";

            BusinessLogicLayer.Reports RptHeader = new BusinessLogicLayer.Reports();

            FileHeader = RptHeader.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), true, true, true, true, true, true) + Environment.NewLine + "Customer Ageing With Days Interval" + Environment.NewLine + "For the period " + Convert.ToDateTime(ASPxAsOnDate.Date).ToString("dd-MM-yyyy");
            //Rev Subhra 24-12-2018   0017670
            FileHeader = ReplaceFirst(FileHeader, "\r\n", Convert.ToString(Session["BranchNames"]));
            //End of Rev

            exporter.PageHeader.Left = FileHeader;
            exporter.PageHeader.Font.Size = 10;
            exporter.PageHeader.Font.Name = "Tahoma";

            exporter.GridViewID = "ShowGridCustAgeingWithDaysInterval";
            exporter.RenderBrick += exporter_RenderBrick;
            switch (Filter)
            {
                case 1:
                    exporter.WritePdfToResponse();
                    break;
                case 2:
                    exporter.WriteXlsxToResponse(new XlsxExportOptionsEx() { ExportType = ExportType.WYSIWYG });
                    break;
                case 3:
                    exporter.WriteRtfToResponse();
                    break;
                case 4:
                    exporter.WriteCsvToResponse();
                    break;

                default:
                    return;
            }

        }
        //Rev Subhra 24-12-2018   0017670
        public string ReplaceFirst(string text, string search, string replace)
        {
            int pos = text.IndexOf(search);
            if (pos < 0)
            {
                return text;
            }
            return text.Substring(0, pos) + Environment.NewLine + replace + Environment.NewLine + text.Substring(pos + search.Length);
        }
        //End of Rev
        protected void exporter_RenderBrick(object sender, ASPxGridViewExportRenderingEventArgs e)
        {
            e.BrickStyle.BackColor = Color.White;
            e.BrickStyle.ForeColor = Color.Black;
        }
        #endregion

        #region Branch Populate

        public void BranchHoOffice()
        {
            CommonBL cbl = new CommonBL();
            DataTable tcbl = new DataTable();
            DataTable dtBranchChild = new DataTable();
            tcbl = cbl.GetBranchheadoffice(Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]), "HO");
            if (tcbl.Rows.Count > 0)
            {
                ddlbranchHO.DataSource = tcbl;
                ddlbranchHO.DataTextField = "Code";
                ddlbranchHO.DataValueField = "branch_id";
                ddlbranchHO.DataBind();
                dtBranchChild = GetChildBranch(Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]));
                if (dtBranchChild.Rows.Count > 0)
                {
                    ddlbranchHO.Items.Insert(0, new ListItem("All", "All"));
                }
            }
        }

        public DataTable GetChildBranch(string CHILDBRANCH)
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            SqlCommand cmd = new SqlCommand("PRC_FINDCHILDBRANCH_REPORT", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CHILDBRANCH", CHILDBRANCH);
            cmd.CommandTimeout = 0;
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            da.Fill(dt);
            cmd.Dispose();
            con.Dispose();
            return dt;
        }

        protected void Componentbranch_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            string FinYear = Convert.ToString(Session["LastFinYear"]);

            if (e.Parameter.Split('~')[0] == "BindComponentGrid")
            {
                DataTable ComponentTable = new DataTable();
                string Hoid = e.Parameter.Split('~')[1];
                if (Hoid != "All")
                {
                    ComponentTable = GetBranch(Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]), Hoid);
                    if (ComponentTable.Rows.Count > 0)
                    {
                        Session["SI_ComponentData_Branch"] = ComponentTable;
                        lookup_branch.DataSource = ComponentTable;
                        lookup_branch.DataBind();
                    }
                    else
                    {
                        Session["SI_ComponentData_Branch"] = ComponentTable;
                        lookup_branch.DataSource = null;
                        lookup_branch.DataBind();
                    }

                }
                else
                {
                    ComponentTable = oDBEngine.GetDataTable("select * from (select branch_id as ID,branch_description,branch_code from tbl_master_branch a where a.branch_id=1  union all select branch_id as ID,branch_description,branch_code from tbl_master_branch b where b.branch_parentId=1) a order by branch_description");
                    if (ComponentTable.Rows.Count > 0)
                    {
                        Session["SI_ComponentData_Branch"] = ComponentTable;
                        lookup_branch.DataSource = ComponentTable;
                        lookup_branch.DataBind();
                    }
                }
            }
        }

        public DataTable GetBranch(string BRANCH_ID, string Ho)
        {
            DataTable dt = new DataTable();
            //SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]);
            SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            SqlCommand cmd = new SqlCommand("GetFinancerBranchfetchhowise", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Branch", BRANCH_ID);
            cmd.Parameters.AddWithValue("@Hoid", Ho);
            cmd.CommandTimeout = 0;
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            da.Fill(dt);
            cmd.Dispose();
            con.Dispose();

            return dt;
        }

        protected void lookup_branch_DataBinding(object sender, EventArgs e)
        {
            if (Session["SI_ComponentData_Branch"] != null)
            {
                //    DataTable ComponentTable = oDBEngine.GetDataTable("select branch_id,branch_description,branch_code from tbl_master_branch where branch_parentId='" + Hoid + "' order by branch_description asc");
                lookup_branch.DataSource = (DataTable)Session["SI_ComponentData_Branch"];
            }
        }

        #endregion

        #region Customer Outstanding Vs. Ageing Analysis grid
        protected void ShowGridCustAgeingWithDaysInterval_CustomCallback(object sender, ASPxGridViewCustomCallbackEventArgs e)
        {
            ShowGridCustAgeingWithDaysInterval.DataSource = null;

            string returnPara = Convert.ToString(e.Parameters);
            string HEAD_BRANCH = returnPara.Split('~')[1];

            string COMPANYID = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
            string Finyear = Convert.ToString(HttpContext.Current.Session["LastFinYear"]);

            DateTime dtFrom;

            dtFrom = Convert.ToDateTime(ASPxAsOnDate.Date);
            string ASONDATE = dtFrom.ToString("yyyy-MM-dd");
            string BRANCH_ID = "";

            string BranchComponent = "";
            List<object> BranchList = lookup_branch.GridView.GetSelectedFieldValues("ID");
            foreach (object Brnch in BranchList)
            {
                BranchComponent += "," + Brnch;
            }
            BRANCH_ID = BranchComponent.TrimStart(',');

           
            string ALLPARTY = (chkallcust.Checked) ? "1" : "0";
            string CBVOUCHER = (chkcb.Checked) ? "1" : "0";
            string JVOUCHER = (chkjv.Checked) ? "1" : "0";
            string DNCNNOTE = (chkdncn.Checked) ? "1" : "0";
            //Rev Subhra 24-12-2018   0017670

            string BRANCH_NAME = "";
            string BranchNameComponent = "";
            List<object> BranchNameList = lookup_branch.GridView.GetSelectedFieldValues("branch_description");
            foreach (object BranchName in BranchNameList)
            {
                BranchNameComponent += "," + BranchName;
            }
            if (BranchNameList.Count > 1)
            {
                BRANCH_NAME = "Multiple Branch Selected";
                Session["BranchNames"] = BRANCH_NAME;
            }
            else
            {
                BRANCH_NAME = BranchNameComponent.TrimStart(',');
                Session["BranchNames"] = "For Unit : " + BRANCH_NAME + " ";
            }
            ShowGridCustAgeingWithDaysInterval.JSProperties["cpBranchNames"] = Convert.ToString(Session["BranchNames"]);

            //End of Rev

            Task PopulateStockTrialDataTask = new Task(() => GetCustAgeingWithDaysIntrvldata(ASONDATE, BRANCH_ID, ALLPARTY, CBVOUCHER, JVOUCHER, DNCNNOTE));
            PopulateStockTrialDataTask.RunSynchronously();
            ShowGridCustAgeingWithDaysInterval.ExpandAll();
        }
        public void GetCustAgeingWithDaysIntrvldata(string ASONDATE, string BRANCH_ID, string ALLPARTY, string CBVOUCHER, string JVOUCHER, string DNCNNOTE)
        {
            try
            {
                DataSet ds = new DataSet();
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("PRC_PARTYWISEAGEINGDETAIL_REPORT", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@COMPANYID", Convert.ToString(Session["LastCompany"]));
                cmd.Parameters.AddWithValue("@FINYEAR", Convert.ToString(Session["LastFinYear"]));
                cmd.Parameters.AddWithValue("@ASONDATE", ASONDATE);
                cmd.Parameters.AddWithValue("@ALLPARTY", ALLPARTY);
                cmd.Parameters.AddWithValue("@BRANCHID", BRANCH_ID);
                cmd.Parameters.AddWithValue("@PARTY_CODE", hdnCustomerId.Value);
                cmd.Parameters.AddWithValue("@NOOFDAYS", Convert.ToDecimal(txt_NoofDays.Text));
                cmd.Parameters.AddWithValue("@DAYSINTERVAL", Convert.ToDecimal(txt_DaysInterval.Text));
                cmd.Parameters.AddWithValue("@PARTY_TYPE", "C");
                cmd.Parameters.AddWithValue("@INCLUDECB", CBVOUCHER);
                cmd.Parameters.AddWithValue("@INCLUDEJV", JVOUCHER);
                cmd.Parameters.AddWithValue("@EXCLUDEDNCN", DNCNNOTE);

                cmd.CommandTimeout = 0;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(ds);

                cmd.Dispose();
                con.Dispose();

                DataTable dt = new DataTable();
                dt = ds.Tables[0].AsEnumerable().Where(row => row.Field<string>("DOC_TYPE") != "Net Total:").CopyToDataTable();
                Session["CustAgeingWithDaysIntrvl"] = dt;
                Session["Intervl"] = "Y";

                DataTable dt_nTot = new DataTable();
                dt_nTot = ds.Tables[0].AsEnumerable().Where(row => row.Field<string>("DOC_TYPE") == "Net Total:").CopyToDataTable();
                Session["CustAgeingTotal"] = dt_nTot;

                ShowGridCustAgeingWithDaysInterval.DataSource = dt;
                ShowGridCustAgeingWithDaysInterval.DataBind();
            }
            catch (Exception ex)
            {

            }
        }

        protected void ShowGridCustAgeingWithDaysInterval_DataBinding(object sender, EventArgs e)
        {
            //string headeramountscaption = "";
            //string headeramountsfield = "";

            ShowGridCustAgeingWithDaysInterval.DataSource = null;

            DataTable dt_CustAgeingWithDaysIntrvl = (DataTable)Session["CustAgeingWithDaysIntrvl"];

            if (dt_CustAgeingWithDaysIntrvl != null)
            {
                int nodys = Convert.ToInt32(txt_DaysInterval.Text);
                int dysint = Convert.ToInt32(txt_NoofDays.Text) / Convert.ToInt32(txt_DaysInterval.Text);
                int interval = Convert.ToInt32(txt_NoofDays.Text) / Convert.ToInt32(txt_DaysInterval.Text);

                int ends_ = Convert.ToInt32(txt_DaysInterval.Text);
                for (int i = 1; i <= interval; i++)
                {

                    int start_ = 1, end_ = Convert.ToInt32(txt_DaysInterval.Text);
                    if (i == 1)
                    {

                    }
                    else
                    {
                        start_ = ends_ * (i - 1) + 1;
                    }
                    end_ = end_ * i;

                    if (!dt_CustAgeingWithDaysIntrvl.Columns.Contains((start_ + "-" + end_ + " DAYS")))
                    {
                        Session["Intervl"] = "N";
                    }

                }              

                if (Session["Intervl"] == "Y")
                {

                    string fieldname = "";
                    string fieldnamecheck = "";
                    GridViewBandColumn bandColumn = new GridViewBandColumn();
                    if (ShowGridCustAgeingWithDaysInterval.Columns["DOC_NO"] == null)
                    {
                        GridViewDataTextColumn col0 = new GridViewDataTextColumn();
                        col0.FieldName = "PARTYNAME";
                        col0.Caption = "Customer Name";
                        col0.CellStyle.HorizontalAlign = HorizontalAlign.Left;
                        col0.Width = 220;
                        col0.GroupIndex = 1;
                        ShowGridCustAgeingWithDaysInterval.Columns.Add(col0);

                        GridViewDataTextColumn col1 = new GridViewDataTextColumn();
                        col1.FieldName = "BRANCH_DESCRIPTION";
                        col1.Caption = "Unit";
                        col1.CellStyle.HorizontalAlign = HorizontalAlign.Left;
                        col1.Width = 220;
                        ShowGridCustAgeingWithDaysInterval.Columns.Add(col1);

                        GridViewDataTextColumn col2 = new GridViewDataTextColumn();
                        col2.FieldName = "DOC_NO";
                        col2.Caption = "Doc. No";
                        col2.CellStyle.HorizontalAlign = HorizontalAlign.Left;
                        col2.Width = 150;
                        ShowGridCustAgeingWithDaysInterval.Columns.Add(col2);

                        GridViewDataTextColumn col3 = new GridViewDataTextColumn();
                        col3.FieldName = "DOC_TYPE";
                        col3.Caption = "Doc. Type";
                        col3.CellStyle.HorizontalAlign = HorizontalAlign.Left;
                        col3.Width = 150;
                        ShowGridCustAgeingWithDaysInterval.Columns.Add(col3);

                        GridViewDataTextColumn col4 = new GridViewDataTextColumn();
                        col4.FieldName = "ISOPENING";
                        col4.Caption = "Opening?";
                        col4.CellStyle.HorizontalAlign = HorizontalAlign.Left;
                        col4.Width = 150;
                        ShowGridCustAgeingWithDaysInterval.Columns.Add(col4);

                        GridViewDataTextColumn col5 = new GridViewDataTextColumn();
                        col5.FieldName = "DOC_DATE";
                        col5.Caption = "Doc. Date";
                        col5.CellStyle.HorizontalAlign = HorizontalAlign.Left;
                        col5.Width = 110;
                        ShowGridCustAgeingWithDaysInterval.Columns.Add(col5);

                        GridViewDataTextColumn col6 = new GridViewDataTextColumn();
                        col6.FieldName = "DUE_DATE";
                        col6.Caption = "Due Date";
                        col6.CellStyle.HorizontalAlign = HorizontalAlign.Left;
                        col6.Width = 150;
                        ShowGridCustAgeingWithDaysInterval.Columns.Add(col6);

                        GridViewDataTextColumn col7 = new GridViewDataTextColumn();
                        col7.FieldName = "DOC_AMOUNT";
                        col7.Caption = "Doc. Amt.";
                        col7.CellStyle.HorizontalAlign = HorizontalAlign.Right;
                        col7.Width = 110;
                        col7.HeaderStyle.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Right;
                        //Rev Debashis Refer:0019573
                        col7.PropertiesTextEdit.DisplayFormatString = "0.00";
                        //End of Rev Debashis
                        ShowGridCustAgeingWithDaysInterval.Columns.Add(col7);
                       
                        GridViewDataTextColumn col8 = new GridViewDataTextColumn();
                        col8.FieldName = "CUMBAL_AMOUNT";
                        col8.Caption = "Balance";
                        col8.CellStyle.HorizontalAlign = HorizontalAlign.Right;
                        col8.Width = 150;
                        col8.HeaderStyle.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Right;
                        //Rev Debashis Refer:0019573
                        col8.PropertiesTextEdit.DisplayFormatString = "0.00";
                        //End of Rev Debashis
                        ShowGridCustAgeingWithDaysInterval.Columns.Add(col8);
                    }

                    hdnNoCaption.Value = "Doc. Amt." + "~" + "Balance";
                    hdnNoFields.Value = "DOC_AMOUNT" + "~" + "CUMBAL_AMOUNT";

                    ShowGridCustAgeingWithDaysInterval.Columns["PARTYNAME"].Width = 220;
                    ShowGridCustAgeingWithDaysInterval.Columns["PARTYNAME"].VisibleIndex = 0;

                    ShowGridCustAgeingWithDaysInterval.Columns["BRANCH_DESCRIPTION"].Width = 220;
                    ShowGridCustAgeingWithDaysInterval.Columns["BRANCH_DESCRIPTION"].VisibleIndex = 1;
                    ShowGridCustAgeingWithDaysInterval.Columns["DOC_NO"].Width = 110;
                    ShowGridCustAgeingWithDaysInterval.Columns["DOC_NO"].VisibleIndex = 2;
                    ShowGridCustAgeingWithDaysInterval.Columns["DOC_TYPE"].Width = 110;
                    ShowGridCustAgeingWithDaysInterval.Columns["DOC_TYPE"].VisibleIndex = 3;
                    ShowGridCustAgeingWithDaysInterval.Columns["ISOPENING"].Width = 110;
                    ShowGridCustAgeingWithDaysInterval.Columns["ISOPENING"].VisibleIndex = 4;
                    ShowGridCustAgeingWithDaysInterval.Columns["DOC_DATE"].Width = 110;
                    ShowGridCustAgeingWithDaysInterval.Columns["DOC_DATE"].VisibleIndex = 5;

                    ShowGridCustAgeingWithDaysInterval.Columns["DUE_DATE"].Width = 110;
                    ShowGridCustAgeingWithDaysInterval.Columns["DUE_DATE"].VisibleIndex = 6;

                    ShowGridCustAgeingWithDaysInterval.Columns["DOC_AMOUNT"].Width = 110;
                    ShowGridCustAgeingWithDaysInterval.Columns["DOC_AMOUNT"].VisibleIndex = 7;


                    GridViewDataTextColumn coldyn = new GridViewDataTextColumn();
                    int j = 1;
                    int ends = Convert.ToInt32(txt_DaysInterval.Text);
                    for (int i = 1; i <= interval; i++)
                    {
                        int start = 1, end = Convert.ToInt32(txt_DaysInterval.Text);
                        if (i == 1)
                        {

                        }
                        else
                        {
                            start = ends * (i - 1) + 1;
                        }
                        end = end * i;
                       
                        if (ShowGridCustAgeingWithDaysInterval.Columns[start + "-" + end + " DAYS"] == null)
                        {
                            fieldname = start + "-" + end + " DAYS";
                            coldyn = new GridViewDataTextColumn();
                            coldyn.FieldName = fieldname;
                            coldyn.Caption = start + "-" + end + " DAYS";
                            coldyn.CellStyle.HorizontalAlign = HorizontalAlign.Right;
                            coldyn.Width = 150;
                            coldyn.VisibleIndex = 7 + j;
                            coldyn.HeaderStyle.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Right;
                            //Rev Debashis Refer:0019573
                            coldyn.PropertiesTextEdit.DisplayFormatString = "0.00";
                            //End of Rev Debashis
                            ShowGridCustAgeingWithDaysInterval.Columns.Add(coldyn);
                            j++;
                        }
                        hdnNoCaption.Value = hdnNoCaption.Value + "~" + start + "-" + end + " DAYS";
                        hdnNoFields.Value = hdnNoFields.Value + "~" + start + "-" + end + " DAYS";
                    }

                   
                    if (dt_CustAgeingWithDaysIntrvl.Columns.Contains("ABOVE " + Convert.ToInt32(nodys * dysint) + " DAYS"))
                    {
                        if (ShowGridCustAgeingWithDaysInterval.Columns["ABOVE " + Convert.ToInt32(nodys * dysint) + " DAYS"] == null)
                        {
                            fieldname = "ABOVE " + Convert.ToInt32(nodys * dysint) + " DAYS";
                            coldyn = new GridViewDataTextColumn();
                            coldyn.FieldName = fieldname;
                            coldyn.Caption = fieldname;
                            coldyn.CellStyle.HorizontalAlign = HorizontalAlign.Right;
                            coldyn.Width = 150;
                            coldyn.VisibleIndex = 7 + j + 1;
                            coldyn.HeaderStyle.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Right;
                            //Rev Debashis Refer:0019573
                            coldyn.PropertiesTextEdit.DisplayFormatString = "0.00";
                            //End of Rev Debashis
                            ShowGridCustAgeingWithDaysInterval.Columns.Add(coldyn);
                            ShowGridCustAgeingWithDaysInterval.Columns["CUMBAL_AMOUNT"].VisibleIndex = 7 + j + 2;
                        }
                    }
                    hdnNoCaption.Value = hdnNoCaption.Value + "~" + "ABOVE " + Convert.ToInt32(nodys * dysint) + " DAYS";
                    hdnNoFields.Value = hdnNoFields.Value + "~" + "ABOVE " + Convert.ToInt32(nodys * dysint) + " DAYS";

                    ShowGridCustAgeingWithDaysInterval.DataSource = dt_CustAgeingWithDaysIntrvl;
                    Session["HeadersAmountCaption"] = hdnNoCaption.Value;
                    Session["HeadersAmountFields"] = hdnNoFields.Value;
                }

            }
        }

        protected void ShowGridCustAgeingWithDaysInterval_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        {
            if (Convert.ToString(e.CellValue) == "Party Total:")
            {
                Session["chk_presenttotal"] = 1;
            }
            if (Convert.ToInt32(Session["chk_presenttotal"]) == 1)
            {
                e.Cell.Font.Bold = true;
                e.Cell.BackColor = Color.DarkSeaGreen;
            }

            if (e.DataColumn.FieldName == "CUMBAL_AMOUNT")
            {
                Session["chk_presenttotal"] = 0;
            }
        }

        protected void ShowGridCustAgeingWithDaysInterval_HtmlFooterCellPrepared(object sender, ASPxGridViewTableFooterCellEventArgs e)
        {
            if(IsPostBack)
            {
                DataTable dt_CustAgeingTotal = (DataTable)Session["CustAgeingTotal"];
                if (dt_CustAgeingTotal != null)
                {
                        string amountfields = Session["HeadersAmountCaption"].ToString();
                        string[] amountfields_caption = amountfields.Split('~');
                        string[] amountfields_fields = (Session["HeadersAmountFields"].ToString()).Split('~');


                        for (int i = 0; i < amountfields_caption.Length; i++)
                        {
                            if (e.Column.Caption == amountfields_caption[i])
                            {
                                if (e.IsTotalFooter)
                                {
                                    e.Cell.Text = dt_CustAgeingTotal.Rows[0][amountfields_fields[i]].ToString();
                                }

                            }
                       }
                }
                if (e.Column.Caption == "Doc. Type")
                {
                    if (e.IsTotalFooter)
                    {
                        e.Cell.Text = "Net Total:";
                    }
                }
            }
        }       
    }

        #endregion
}
