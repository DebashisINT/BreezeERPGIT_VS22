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
    public partial class FinanceCNReport : System.Web.UI.Page
    {
        DataTable DTIndustry = new DataTable();
        BusinessLogicLayer.Reports objReport = new BusinessLogicLayer.Reports();
        //string data = "";

        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine(string.Empty);

        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();

        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string sPath = Convert.ToString(HttpContext.Current.Request.Url);
                oDBEngine.Call_CheckPageaccessebility(sPath);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/management/master/FinanceCNReport.aspx");
            DateTime dtFrom;
            DateTime dtTo;

            if (!IsPostBack)
            {
                DataTable dtBranchSelection = new DataTable();
                string GridHeader = "";
                BusinessLogicLayer.Reports GridHeaderDet = new BusinessLogicLayer.Reports();
                RptHeading.Text = "Finance Register";
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

                //Session.Remove("dtLedger");

                //ShowGrid.JSProperties["cpSave"] = null;
                Session["exportval"] = null;
                //Session["dtLedger"] = null;
                Session["IsFinanceRegFilter"] = null;
                Session["SI_ComponentData_Financer"] = null;
                Session["SI_ComponentData_Branch"] = null;

                dtFrom = DateTime.Now;
                dtTo = DateTime.Now;
                ASPxFromDate.Text = dtFrom.ToString("dd-MM-yyyy");
                ASPxToDate.Text = dtTo.ToString("dd-MM-yyyy");

                ASPxFromDate.Value = DateTime.Now;
                ASPxToDate.Value = DateTime.Now;
                BranchHoOffice();
                Date_finyearwise(Convert.ToString(Session["LastFinYear"]));
                //Rev Subhra 18-12-2018   0017670
                Session["BranchNames"] = null;
                //End Rev
                dtBranchSelection = oDBEngine.GetDataTable("select Variable_Value from Config_SystemSettings where Variable_Name='ReportsBranchSelection'");
                hdnBranchSelection.Value = dtBranchSelection.Rows[0][0].ToString();

                //string COMPANYID = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
                //string Finyear = Convert.ToString(HttpContext.Current.Session["LastFinYear"]);
                //dtFrom = Convert.ToDateTime(ASPxFromDate.Date);
                //dtTo = Convert.ToDateTime(ASPxToDate.Date);
                //string FROMDATE = dtFrom.ToString("yyyy-MM-dd");
                //string TODATE = dtTo.ToString("yyyy-MM-dd");
                //string CUSTVENDID = "";
                //if (hdnSelectedCustomerVendor.Value != "")
                //{
                //    CUSTVENDID = hdnSelectedCustomerVendor.Value;
                //}
                ///GetLedgerdata(FROMDATE, TODATE,  BRANCH_ID,   CUSTVENDID);
                //Task PopulateStockTrialDataTask = new Task(() => GetLedgerdata(FROMDATE, TODATE, BRANCH_ID, CUSTVENDID));
                //PopulateStockTrialDataTask.RunSynchronously(); 
            }
            else
            {
                dtFrom = Convert.ToDateTime(ASPxFromDate.Date);
                dtTo = Convert.ToDateTime(ASPxToDate.Date);
            }
            if (!IsPostBack)
            {
                string COMPANYID = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
                string Finyear = Convert.ToString(HttpContext.Current.Session["LastFinYear"]);
                dtFrom = Convert.ToDateTime(ASPxFromDate.Date);
                dtTo = Convert.ToDateTime(ASPxToDate.Date);

                string FROMDATE = dtFrom.ToString("yyyy-MM-dd");
                string TODATE = dtTo.ToString("yyyy-MM-dd");
                string BRANCH_ID = "";
                if (hdnSelectedBranches.Value != "")
                {
                    BRANCH_ID = hdnSelectedBranches.Value;
                }
            }
        }

        public void cmbExport_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 Filter = int.Parse(Convert.ToString(drdExport.SelectedItem.Value));
            if (Filter != 0)
            {
                BranchHoOffice();
                if (Session["exportval"] == null)
                {
                    Session["exportval"] = Filter;
                    // BindDropDownList();
                    bindexport(Filter);
                }
                else if (Convert.ToInt32(Session["exportval"]) != Filter)
                {
                    Session["exportval"] = Filter;
                    // BindDropDownList();
                    bindexport(Filter);
                }
                //drdExport.SelectedValue = "0";
            }

        }

        public void bindexport(int Filter)
        {
            string filename = "Finance Register";
            exporter.FileName = filename;
            string FileHeader = "";

            exporter.FileName = filename;

            BusinessLogicLayer.Reports RptHeader = new BusinessLogicLayer.Reports();

            FileHeader = RptHeader.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), true, true, true,true, true, true) + Environment.NewLine + "Finance Register" + Environment.NewLine + "For the period " + Convert.ToDateTime(ASPxFromDate.Date).ToString("dd-MM-yyyy") + " To " + Convert.ToDateTime(ASPxToDate.Date).ToString("dd-MM-yyyy");
            //Rev Subhra 18-12-2018   0017670
            FileHeader = ReplaceFirst(FileHeader, "\r\n", Convert.ToString(Session["BranchNames"]));
            //End of Rev
            exporter.PageHeader.Left = FileHeader;
            exporter.PageHeader.Font.Size = 10;
            exporter.PageHeader.Font.Name = "Tahoma";

            //exporter.PageFooter.Center = "[Page # of Pages #]";
            //exporter.PageFooter.Right = "[Date Printed]";
            exporter.Landscape = true;
            exporter.MaxColumnWidth = 100;
            exporter.GridViewID = "ShowGrid";

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
            }

        }

        //Rev Subhra 18-12-2018   0017670
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

        //protected void Grid_CustomCallback(object sender, DevExpress.Web.ASPxGridViewCustomCallbackEventArgs e)
        protected void CallbackPanel_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            string returnPara = Convert.ToString(e.Parameter);
            string HEAD_BRANCH = returnPara.Split('~')[1];
            //Session.Remove("dtLedger");
            //ShowGrid.JSProperties["cpSave"] = null;
            //  string[] CallVal = e.Parameters.ToString().Split('~');

            string IsFinanceRegFilter = Convert.ToString(hfIsFinanceRegFilter.Value);
            Session["IsFinanceRegFilter"] = IsFinanceRegFilter;

            string COMPANYID = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
            string Finyear = Convert.ToString(HttpContext.Current.Session["LastFinYear"]);

            DateTime dtFrom;
            DateTime dtTo;
            dtFrom = Convert.ToDateTime(ASPxFromDate.Date);
            dtTo = Convert.ToDateTime(ASPxToDate.Date);

            string FROMDATE = dtFrom.ToString("yyyy-MM-dd");
            string TODATE = dtTo.ToString("yyyy-MM-dd");

            //string TABLENAME = "Ledger Details";         
            //if (hdnSelectedBranches.Value != "")
            //{
            //    BRANCH_ID=hdnSelectedBranches.Value;
            //}
            string BRANCH_ID = "";
            string QuoComponent = "";
            List<object> QuoList = lookup_branch.GridView.GetSelectedFieldValues("ID");
            foreach (object Quo in QuoList)
            {
                QuoComponent += "," + Quo;
            }
            BRANCH_ID = QuoComponent.TrimStart(',');

            string FINANCERID = "";
            FINANCERID = hdnFinancerId.Value;
            //string CASHBANKTYPE = "";
            //string CASHBANKID = "";
            //string UserId = "";            
            //if (hdnSelectedCustomerVendor.Value != "")
            //{
            //    CUSTVENDID = hdnSelectedCustomerVendor.Value;
            //}            
            //string QuoComponent2 = "";
            //List<object> QuoList2 = gridfinancerLookup.GridView.GetSelectedFieldValues("ID");
            //foreach (object Quo2 in QuoList2)
            //{
            //    QuoComponent2 += "," + Quo2;
            //}
            //CUSTVENDID = QuoComponent2.TrimStart(',');

            //string LEDGERID = "";
            //if(hdnSelectedLedger.Value!="")
            //{
            //    LEDGERID = hdnSelectedLedger.Value;
            //}
            //string ISCREATEORPREVIEW = "P";
            /// GetLedgerdata(FROMDATE, TODATE, BRANCH_ID, CUSTVENDID);

            //Rev Subhra 18-12-2018   0017670

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
            CallbackPanel.JSProperties["cpBranchNames"] = Convert.ToString(Session["BranchNames"]);

            //End of Rev

            Task PopulateStockTrialDataTask = new Task(() => GetFinanceRegisterdata(FROMDATE, TODATE, BRANCH_ID, FINANCERID, HEAD_BRANCH));
            PopulateStockTrialDataTask.RunSynchronously();
        }

        public void GetFinanceRegisterdata(string FROMDATE, string TODATE, string BRANCH_ID, string FINANCERID, string HEAD_BRANCH)
        {
            try
            {
                //string DriverName = string.Empty;
                //string PhoneNo = string.Empty;
                //string VehicleNo = string.Empty;

                DataSet ds = new DataSet();
                //SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]);
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                ///SqlCommand cmd = new SqlCommand("prc_FinanceRegister_Report", con);
                SqlCommand cmd = new SqlCommand("prc_FinanceRegister_Report", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@COMPANYID", Convert.ToString(Session["LastCompany"]));
                cmd.Parameters.AddWithValue("@FINYEAR", Convert.ToString(Session["LastFinYear"]).Trim());
                cmd.Parameters.AddWithValue("@FROMDATE", FROMDATE);
                cmd.Parameters.AddWithValue("@TODATE", TODATE);
                cmd.Parameters.AddWithValue("@BRANCHID", BRANCH_ID);
                cmd.Parameters.AddWithValue("@P_CODE", FINANCERID);
                cmd.Parameters.AddWithValue("@outstanding", ddlfinanceout.SelectedValue);
                //cmd.Parameters.AddWithValue("@HO", ddlbranchHO.SelectedValue);
                cmd.Parameters.AddWithValue("@HO", HEAD_BRANCH);
                cmd.Parameters.AddWithValue("@USERID", Convert.ToInt32(Session["userid"]));

                cmd.CommandTimeout = 0;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(ds);

                cmd.Dispose();
                con.Dispose();

                //Session["dtLedger"] = ds.Tables[0];
                //if (ds.Tables[0].Rows.Count > 0)
                //{
                //    ShowGrid.DataSource = ds.Tables[0];
                //    ShowGrid.DataBind();
                //}
                //else
                //{
                //    Session["dtLedger"] = null;
                //    ShowGrid.DataSource = null;
                //    ShowGrid.DataBind();

                //}
            }
            catch (Exception ex)
            {
            }
        }

        //protected void ShowGrid_DataBinding(object sender, EventArgs e)
        //{
        //    if (Session["dtLedger"] != null)
        //    {
        //        ShowGrid.DataSource = (DataTable)Session["dtLedger"];
        //        //  ShowGrid.DataBind();
        //    }

        //}

        public void Date_finyearwise(string Finyear)
        {
            CommonBL cmbl = new CommonBL();
            DataTable dtfnyear = new DataTable();

            dtfnyear = cmbl.GetDateFinancila(Finyear);
            if (dtfnyear.Rows.Count > 0)
            {
                ASPxFromDate.MaxDate = Convert.ToDateTime((dtfnyear.Rows[0]["FinYear_EndDate"]));
                ASPxFromDate.MinDate = Convert.ToDateTime((dtfnyear.Rows[0]["FinYear_StartDate"]));

                ASPxToDate.MaxDate = Convert.ToDateTime((dtfnyear.Rows[0]["FinYear_EndDate"]));
                ASPxToDate.MinDate = Convert.ToDateTime((dtfnyear.Rows[0]["FinYear_StartDate"]));

                DateTime MaximumDate = Convert.ToDateTime((dtfnyear.Rows[0]["FinYear_EndDate"]));
                DateTime MinimumDate = Convert.ToDateTime((dtfnyear.Rows[0]["FinYear_StartDate"]));

                DateTime TodayDate = Convert.ToDateTime(DateTime.Now);
                DateTime FinYearEndDate = MaximumDate;

                if (TodayDate > FinYearEndDate)
                {
                    ASPxToDate.Date = FinYearEndDate;
                    ASPxFromDate.Date = MinimumDate;
                }
                else
                {
                    ASPxToDate.Date = TodayDate;
                    ASPxFromDate.Date = MinimumDate;
                }

            }
        }

        //public static List<string> GetLedgerBind(string branch)
        //{
        //    CommonBL bll1 = new CommonBL();
        //    DataTable stbill = new DataTable();
        //    stbill = bll1.GetLedgerBind(branch);
        //    List<string> obj = new List<string>();
        //    if (stbill.Rows.Count > 0)
        //    {
        //        foreach (DataRow dr in stbill.Rows)
        //        {

        //            obj.Add(Convert.ToString(dr["AccountName"]) + "|" + Convert.ToString(dr["Id"]));
        //        }
        //    }

        //    return obj;

        //}
        #region LinQ
        protected void GenerateEntityServerModeDataSource_Selecting(object sender, DevExpress.Data.Linq.LinqServerModeDataSourceSelectEventArgs e)
        {
            e.KeyExpression = "SEQ";
            //string connectionString = ConfigurationManager.ConnectionStrings["crmConnectionString"].ConnectionString;
            SqlConnection connectionString = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            string Userid = Convert.ToString(HttpContext.Current.Session["userid"]);
            ReportSourceDataContext dc = new ReportSourceDataContext(connectionString);

            if (Convert.ToString(Session["IsFinanceRegFilter"]) == "Y")
            {
                var q = from d in dc.FINANCEREGISTER_REPORTs
                        where Convert.ToString(d.USERID) == Userid && Convert.ToString(d.OUTSTANDINGTYPE) == ddlfinanceout.SelectedValue
                        orderby d.SEQ
                        select d;
                e.QueryableSource = q;
            }
            else
            {
                var q = from d in dc.FINANCEREGISTER_REPORTs
                        where Convert.ToString(d.SEQ) == "0"
                        orderby d.SEQ
                        select d;
                e.QueryableSource = q;
            }
        }
        #endregion
        protected void Showgrid_Htmlprepared(object sender, EventArgs e)
        {
            ASPxGridView grid = (ASPxGridView)sender;
            if (ddlfinanceout.SelectedValue == "1")
            {
                grid.Columns["Excess"].Visible = false;
                grid.Columns["ShortAmt"].Visible = false;
                grid.Columns["Final_Pay"].Visible = false;
                grid.Columns["Chq_No"].Visible = false;
                grid.Columns["Finance_Amt"].Visible = false;
                grid.Columns["Processing_Fee"].Visible = false;
                grid.Columns["Arn_No"].Visible = false;
                //    grid.Columns["DBD Amt."].Visible = false;
                grid.Columns["MBD_Percentage"].Visible = false;
                grid.Columns["MBD_Amt"].Visible = false;
            }
            else
            {
                grid.Columns["Excess"].Visible = true;
                grid.Columns["ShortAmt"].Visible = true;
                grid.Columns["Final_Pay"].Visible = true;
                grid.Columns["Chq_No"].Visible = true;
                grid.Columns["Finance_Amt"].Visible = true;
                grid.Columns["Processing_Fee"].Visible = true;
                grid.Columns["Arn_No"].Visible = true;
                //      grid.Columns["DBD Amt."].Visible = true;
                grid.Columns["MBD_Percentage"].Visible = true;
                grid.Columns["MBD_Amt"].Visible = true;
            }
        }

        protected void ShowGrid_SummaryDisplayText(object sender, ASPxGridViewSummaryDisplayTextEventArgs e)
        {
            e.Text = string.Format("{0}", e.Value);
        }

        #region Branch Populate

        public void BranchHoOffice()
        {
            CommonBL bll1 = new CommonBL();
            DataTable stbill = new DataTable();
            //Rev Debashis && Hierarchy wise Head Branch Bind
            //stbill = bll1.GetBranchheadoffice("HO");
            DataTable dtBranchChild = new DataTable();
            stbill = bll1.GetBranchheadoffice(Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]), "HO");
            //End of Rev Debashis
            if (stbill.Rows.Count > 0)
            {
                ddlbranchHO.DataSource = stbill;
                ddlbranchHO.DataTextField = "Code";
                ddlbranchHO.DataValueField = "branch_id";
                ddlbranchHO.DataBind();
                //Rev Debashis && Hierarchy wise Head Branch Bind
                //ddlbranchHO.Items.Insert(0, new ListItem("All", "All"));
                dtBranchChild = GetChildBranch(Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]));
                if (dtBranchChild.Rows.Count > 0)
                {
                    ddlbranchHO.Items.Insert(0, new ListItem("All", "All"));
                }
                //End of Rev Debashis
            }
        }

        //Rev Debashis && Hierarchy wise Head Branch Bind
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
        //End of Rev Debashis

        protected void Componentbranch_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            string FinYear = Convert.ToString(Session["LastFinYear"]);

            if (e.Parameter.Split('~')[0] == "BindComponentGrid")
            {
                DataTable ComponentTable = new DataTable();
                string Hoid = e.Parameter.Split('~')[1];
                if (Hoid != "All")
                {
                    //if (Session["userbranchHierarchy"] != null)
                    //{
                    //    ComponentTable = oDBEngine.GetDataTable("select branch_id as ID,branch_description,branch_code from tbl_master_branch   where branch_id in(" + Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]) + ")   order by branch_description asc");
                    //}


                    ///  ComponentTable = oDBEngine.GetDataTable("select branch_id as ID,branch_description,branch_code from tbl_master_branch where branch_parentId='" + Hoid + "' and  branch_id in(" + Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]) + ")  order by branch_description asc");

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
                    ComponentTable = oDBEngine.GetDataTable("select * from(select branch_id as ID,branch_description,branch_code from tbl_master_branch a where a.branch_id=1 union all select branch_id as ID,branch_description,branch_code from tbl_master_branch b where b.branch_parentId=1) a order by branch_description");
                    Session["SI_ComponentData_Branch"] = ComponentTable;
                    lookup_branch.DataSource = ComponentTable;
                    lookup_branch.DataBind();

                }
            }
        }

        protected void lookup_branch_DataBinding(object sender, EventArgs e)
        {
            if (Session["SI_ComponentData_Branch"] != null)
            {
                //    DataTable ComponentTable = oDBEngine.GetDataTable("select branch_id,branch_description,branch_code from tbl_master_branch where branch_parentId='" + Hoid + "' order by branch_description asc");
                lookup_branch.DataSource = (DataTable)Session["SI_ComponentData_Branch"];
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


        #endregion

        //#region Financer Populate

        //protected void Componentfinancer_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        //{
        //    string FinYear = Convert.ToString(Session["LastFinYear"]);
        //    string BRANCH_ID = "";
        //    string QuoComponent = "";
        //    List<object> QuoList = lookup_branch.GridView.GetSelectedFieldValues("ID");
        //    foreach (object Quo in QuoList)
        //    {
        //        QuoComponent += "," + Quo;
        //    }
        //    BRANCH_ID = QuoComponent.TrimStart(',');

        //    if (e.Parameter.Split('~')[0] == "clean")
        //    {
        //        gridfinancerLookup.DataSource = null;
        //        gridfinancerLookup.DataBind();

        //    }

        //    if (e.Parameter.Split('~')[0] == "BindComponentGrid")
        //    {
        //        DataTable ComponentTable = new DataTable();
        //        string Hoid = e.Parameter.Split('~')[1];

        //        gridfinancerLookup.DataSource = null;
        //        gridfinancerLookup.DataBind();


        //        //ComponentTable = oDBEngine.GetDataTable("select cnt_internalId AS ID,ISNULL(cnt_firstName,'')+' '+ISNULL(cnt_middleName,'')+'  '+ISNULL(cnt_lastName,'') +'('+tbl_master_branch.branch_description+')' as 'Name'  FROM tbl_master_contact inner join tbl_master_branch on tbl_master_contact.cnt_branchid=tbl_master_branch.branch_id and cnt_contactType in('FI') ORDER BY ISNULL(cnt_firstName,'')+' '+ISNULL(cnt_middleName,'')+' '+ISNULL(cnt_lastName,'') ");
        //        if (string.IsNullOrEmpty(BRANCH_ID))
        //        {
        //            if (Hoid != "All")
        //            {
        //                ComponentTable = GetBranchwiseFinancer(Hoid);
        //            }
        //        }
        //        else
        //        {
        //            ComponentTable = GetBranchwiseFinancer(BRANCH_ID);
        //        }

        //        if (ComponentTable.Rows.Count > 0)
        //        {

        //            Session["SI_ComponentData_Financer"] = ComponentTable;
        //            gridfinancerLookup.DataSource = ComponentTable;
        //            gridfinancerLookup.DataBind();


        //        }
        //        else
        //        {
        //            Session["SI_ComponentData_Financer"] = null;
        //            gridfinancerLookup.DataSource = null;
        //            gridfinancerLookup.DataBind();

        //        }
        //    }
        //}


        //public DataTable GetBranchwiseFinancer(string BRANCH_ID)
        //{
        //    DataTable dt = new DataTable();
        //    SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]);
        //    SqlCommand cmd = new SqlCommand("GetFinancerBranchwise", con);
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.Parameters.AddWithValue("@Branch", BRANCH_ID);
        //    cmd.CommandTimeout = 0;
        //    SqlDataAdapter da = new SqlDataAdapter();
        //    da.SelectCommand = cmd;
        //    da.Fill(dt);
        //    cmd.Dispose();
        //    con.Dispose();

        //    return dt;
        //}



        //protected void lookup_financer_DataBinding(object sender, EventArgs e)
        //{
        //    if (Session["SI_ComponentData_Financer"] != null)
        //    {
        //        //    DataTable ComponentTable = oDBEngine.GetDataTable("select branch_id,branch_description,branch_code from tbl_master_branch where branch_parentId='" + Hoid + "' order by branch_description asc");
        //        gridfinancerLookup.DataSource = (DataTable)Session["SI_ComponentData_Financer"];
        //    }
        //}

        //#endregion

    }
}