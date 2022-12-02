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
using DevExpress.Data;
using System.Threading.Tasks;
using DevExpress.XtraPrinting;
using DevExpress.Export;
using System.Drawing;
using DataAccessLayer;
using Reports.Model;
using DevExpress.XtraPrintingLinks;
using System.IO;
using System.Diagnostics;
using System.Drawing.Printing;
using DevExpress.XtraReports.UI;
using DevExpress.XtraReports.Web;
namespace Reports.Reports.GridReports
{
    public partial class BSStatementProject : System.Web.UI.Page
    {
        BusinessLogicLayer.DBEngine oDBEngine = new BusinessLogicLayer.DBEngine();
        DateTime dtFrom;
        DateTime dtTo;
        static int PageStart = 1;
        public EntityLayer.CommonELS.UserRightsForPage rights = new UserRightsForPage();

        protected void Page_PreInit(object sender, EventArgs e) // lead add
        {
            if (Request.QueryString.AllKeys.Contains("dashboard"))
            {
                this.Page.MasterPageFile = "~/OMS/MasterPage/PopUp.Master";
                //divcross.Visible = false;
            }
            else
            {
                this.Page.MasterPageFile = "~/OMS/MasterPage/ERP.Master";
                //divcross.Visible = true;
            }

        }
        protected void Page_Load(object sender, EventArgs e)
        {
            //if(!IsPostBack)
            rights = BusinessLogicLayer.CommonBLS.CommonBL.GetUserRightSession("/GridReports/BSStatement.aspx");
            if (!IsPostBack)
            {
                TreeList.Columns["Percentage"].Visible = false;
                TreeList1.Columns["Percentage"].Visible = false;
                PageStart = 1;
                Session["BSdata"] = null;
                Session["BSZooming"] = null;
                Session["BSZooming1"] = null;
                Session["BSSTockSummary"] = null;
                Session["BSStockDetails"] = null;

                //TreeList.CollapseAll();
                //TreeList1.CollapseAll();
                radAsDate.Attributes.Add("OnClick", "DateAll('all')");
                radPeriod.Attributes.Add("OnClick", "DateAll('Selc')");

                string COMPANYID = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
                string Finyear = Convert.ToString(HttpContext.Current.Session["LastFinYear"]);
                dtFrom = DateTime.Now;
                dtTo = DateTime.Now;
                ASPxFromDate.Text = dtFrom.ToString("dd-MM-yyyy");
                ASPxToDate.Text = dtTo.ToString("dd-MM-yyyy");
                string BRANCH_ID = "";
                if (hdnSelectedBranches.Value != "")
                {
                    BRANCH_ID = hdnSelectedBranches.Value;
                }
                BranchHoOffice();

            }



            TreeList.DataBind();
            TreeList1.DataBind();
            PageStart = 2;




        }

        protected void ASPxTreeList1_DataBinding(object sender, EventArgs e)
        {

            DataSet dstPl = GetPlData();
            if (dstPl != null && dstPl.Tables.Count > 1)
            {
                TreeList.DataSource = dstPl.Tables[0];
                TreeList1.DataSource = dstPl.Tables[1];
            }
        }

        protected void ASPxTreeList1_HtmlRowPrepared(object sender, DevExpress.Web.ASPxTreeList.TreeListHtmlRowEventArgs e)
        {
            if (e.GetValue("ID").ToString().Contains("G"))
                e.Row.BackColor = Color.FromArgb(211, 235, 183);
            if (e.GetValue("ID").ToString().Contains("TOTAL LOSS"))
            {
                e.Row.ForeColor = Color.Red;
                e.Row.Style.Add("font-weight", "bold");
            }
            if (e.GetValue("ID").ToString().Contains("Actual TOT"))
            {
                e.Row.ForeColor = Color.White;
                e.Row.BackColor = Color.Black;
                e.Row.Style.Add("font-weight", "bold");

            }

            if (e.GetValue("ID").ToString().Contains("TOTAL PROFIT"))
            {
                e.Row.ForeColor = Color.Green;
                e.Row.Style.Add("font-weight", "bold");

            }


            if (e.GetValue("ID").ToString().Contains("M"))
            {
                // e.Row.Cells[3].Text = e.GetValue("AMOUNT").ToString() + " &nbsp;&nbsp;&nbsp;&nbsp         ";
            }



        }

        DataSet GetPlData()
        {
            if (PageStart != 1)
            {
                if (Session["BSdata"] != null)
                {
                    return (DataSet)Session["BSdata"];
                }
                else
                {
                    string branch = "";
                    List<object> lstBranch = lookup_branch.GridView.GetSelectedFieldValues("ID");
                    branch = string.Join(",", lstBranch);

                    string project = "";
                    List<object> lstproject = lookup_project.GridView.GetSelectedFieldValues("ID");
                    project = string.Join(",", lstproject);


                    bool Period = radPeriod.Checked;
                    DataSet dsInst = new DataSet();

                    if (branch != "")
                    {
                        SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                        SqlCommand cmd = new SqlCommand("prcR_BSStatementProject", con);
                        cmd.Parameters.AddWithValue("@FromDate", ASPxFromDate.Date.ToString("yyyy-MM-dd"));
                        cmd.Parameters.AddWithValue("@Todate", ASPxToDate.Date.ToString("yyyy-MM-dd"));
                        cmd.Parameters.AddWithValue("@Branch", branch);
                        cmd.Parameters.AddWithValue("@Valuation", ddlValTech.SelectedValue);
                        cmd.Parameters.AddWithValue("@ShowZero", chkZero.Checked);
                        cmd.Parameters.AddWithValue("@ClosingStock", chkClosingStock.Checked);
                        cmd.Parameters.AddWithValue("@chkConsiderOverhead", chkConsiderOverhead.Checked);
                        cmd.Parameters.AddWithValue("@Period", Period);
                        cmd.Parameters.AddWithValue("@PROJECT_IDS", project);
                        cmd.Parameters.AddWithValue("@USERID", Convert.ToString(Session["userid"]));

                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 0;
                        SqlDataAdapter Adap = new SqlDataAdapter();
                        Adap.SelectCommand = cmd;
                        Adap.Fill(dsInst);
                        cmd.Dispose();
                        con.Dispose();
                        Session["BSdata"] = dsInst;
                    }
                    return dsInst;
                }
            }
            else
            {
                return null;
            }

        }

        protected void ASPxTreeList1_CustomCallback(object sender, DevExpress.Web.ASPxTreeList.TreeListCustomCallbackEventArgs e)
        {
            string parameter = e.Argument;

            if (parameter == "BindGrid")
            {
                Session["BSdata"] = null;
                TreeList.DataBind();
                TreeList.CollapseAll();
                if (chkPercentage.Checked)
                {
                    TreeList.Columns["Percentage"].Visible = true;
                }
                else
                {
                    TreeList.Columns["Percentage"].Visible = false;
                }
            }
            else if (parameter == "ExpandAll")
            {
                TreeList.ExpandAll();
            }
            else if (parameter == "CollapseAll")
            {
                TreeList.CollapseAll();
            }

            else if (parameter == "ShowPercentage")
            {
                TreeList.Columns["Percentage"].Visible = true;
            }
            else if (parameter == "HidePercentage")
            {
                TreeList.Columns["Percentage"].Visible = false;
            }
        }

        protected void ASPxTreeList2_CustomCallback(object sender, DevExpress.Web.ASPxTreeList.TreeListCustomCallbackEventArgs e)
        {
            string parameter = e.Argument;
            if (parameter == "BindGrid")
            {
                Session["BSdata"] = null;
                TreeList1.DataBind();
                if (chkPercentage.Checked)
                {
                    TreeList1.Columns["Percentage"].Visible = true;
                }
                else
                {
                    TreeList1.Columns["Percentage"].Visible = false;
                }

                TreeList1.CollapseAll();
            }
            else if (parameter == "ExpandAll")
            {
                TreeList1.ExpandAll();
            }
            else if (parameter == "CollapseAll")
            {
                TreeList1.CollapseAll();
            }
            else if (parameter == "ShowPercentage")
            {
                TreeList1.Columns["Percentage"].Visible = true;
            }
            else if (parameter == "HidePercentage")
            {
                TreeList1.Columns["Percentage"].Visible = false;
            }

        }



        #region Copy Old PL

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

        public void Date_finyearwise(string Finyear)
        {
            CommonBL bll1 = new CommonBL();
            DataTable stbill = new DataTable();
            DateTime MinDate, MaxDate;

            stbill = bll1.GetDateFinancila(Finyear);
            if (stbill.Rows.Count > 0)
            {
                ASPxFromDate.MaxDate = Convert.ToDateTime((stbill.Rows[0]["FinYear_EndDate"]));
                ASPxFromDate.MinDate = Convert.ToDateTime((stbill.Rows[0]["FinYear_StartDate"]));

                ASPxToDate.MaxDate = Convert.ToDateTime((stbill.Rows[0]["FinYear_EndDate"]));
                ASPxToDate.MinDate = Convert.ToDateTime((stbill.Rows[0]["FinYear_StartDate"]));

                DateTime MaximumDate = Convert.ToDateTime((stbill.Rows[0]["FinYear_EndDate"]));
                DateTime MinimumDate = Convert.ToDateTime((stbill.Rows[0]["FinYear_StartDate"]));

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


        public void BindDropDownList()
        {
            // Declare a Dictionary to hold all the Options with Value and Text.
            Dictionary<string, string> options = new Dictionary<string, string>();
            options.Add("0", "Export to");
            options.Add("1", "PDF");
            options.Add("2", "XLSX");
            options.Add("3", "RTF");
            options.Add("4", "CSV");


            // Bind the Dictionary to the DropDownList.
            //drdExport.DataSource = options;
            //drdExport.DataTextField = "value";
            //drdExport.DataValueField = "key";
            //drdExport.DataBind();
            //drdExport.SelectedValue = "0";
        }

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


        #region Branch Populate

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
                    CommonBL objCommon = new CommonBL();
                    string Setting = objCommon.GetSystemSettingsResult("BranchHierarchyInReport(ForAllBranch)");
                    if (Setting.ToUpper() == "YES")
                        ComponentTable = oDBEngine.GetDataTable("select * from(select branch_id as ID,branch_description,branch_code from tbl_master_branch a where a.branch_id=1 union all select branch_id as ID,branch_description,branch_code from tbl_master_branch b where b.branch_parentId=1) a where ID in (" + Convert.ToString(HttpContext.Current.Session["userbranchHierarchy"]) + ") order by branch_description");
                    else
                        ComponentTable = oDBEngine.GetDataTable("select * from(select branch_id as ID,branch_description,branch_code from tbl_master_branch a where a.branch_id=1 union all select branch_id as ID,branch_description,branch_code from tbl_master_branch b where b.branch_parentId=1) a order by branch_description");

                    Session["SI_ComponentData_Branch"] = ComponentTable;
                    lookup_branch.DataSource = ComponentTable;
                    lookup_branch.DataBind();
                }
            }
        }
        public DataTable GetBranch(string BRANCH_ID, string Ho)
        {

            DataTable dt = new DataTable();
            if (!string.IsNullOrEmpty(Ho) && Ho != "null")
            {
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
            }
            return dt;
        }
        protected void lookup_branch_DataBinding(object sender, EventArgs e)
        {
            if (Session["SI_ComponentData_Branch"] != null)
            {
                lookup_branch.DataSource = (DataTable)Session["SI_ComponentData_Branch"];
            }
        }


        #endregion

        //protected void ShowGridDetails2Level_DataBinding(object sender, EventArgs e)
        //{
        //    if (Session["dt_PLLeve2"] != null)
        //    {
        //        ShowGridDetails2Level.DataSource = (DataTable)Session["dt_PLLeve2"];
        //    }
        //    else
        //    {
        //        ShowGridDetails2Level.DataSource = null;
        //    }
        //}

        protected void CallbackPanelDetail_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            string ledger;
            string asondatewise;
            string ledgerDesc;
            string ledgertype;
            string[] CallVal2ndlevel = e.Parameter.ToString().Split('~');
            ledger = CallVal2ndlevel[0];
            Session["LedgerId"] = ledger;
            asondatewise = Convert.ToString(CallVal2ndlevel[1]);
            ledgertype = CallVal2ndlevel[3];
            Session["ledgertype"] = ledgertype;

            DataTable dtledgdesc = null;
            ledgerDesc = "";

            string IsProfnLossDetailsFilter = Convert.ToString(hfIsProfnlossDetails.Value);
            Session["IsProfnLossDetailsFilter"] = IsProfnLossDetailsFilter;

            if (ledger != "null" && ledger != "0" && ledger != "")
            {
                if (ledgertype != "FOR BRANCH")
                {
                    dtledgdesc = oDBEngine.GetDataTable("Select MainAccount_Name from Master_MainAccount Where CONVERT(NVARCHAR(MAX),MainAccount_ReferenceID)='" + ledger + "'");
                }
                else
                {
                    dtledgdesc = oDBEngine.GetDataTable("Select branch_description from tbl_master_branch Where branch_id='" + ledger + "'");
                }

                ledgerDesc = dtledgdesc.Rows[0][0].ToString();
            }
            else
            {
                dtledgdesc = null;
                ledgerDesc = null;
            }


            if (!string.IsNullOrEmpty(ledger) && ledger != "0")
            {
                //Session.Remove("dt_PLLeve2");
                string COMPANYID = Convert.ToString(HttpContext.Current.Session["LastCompany"]);
                string Finyear = Convert.ToString(HttpContext.Current.Session["LastFinYear"]);

                DateTime dtFrom;
                dtFrom = Convert.ToDateTime(ASPxFromDate.Date);

                string FROMDATE = dtFrom.ToString("yyyy-MM-dd");

                DateTime dtTo;
                dtTo = Convert.ToDateTime(ASPxToDate.Date);

                string TODATE = dtTo.ToString("yyyy-MM-dd");

                string BRANCH_ID = "";

                string QuoComponent = "";
                List<object> QuoList = lookup_branch.GridView.GetSelectedFieldValues("ID");
                foreach (object Quo in QuoList)
                {
                    QuoComponent += "," + Quo;
                }
                BRANCH_ID = QuoComponent.TrimStart(',');

                string branchid = Convert.ToString(e.Parameter.Split('~')[2]);

                DataTable dt = new DataTable();
                dt = GetGeneralLedger2ndLevel(FROMDATE, TODATE, ledger, asondatewise, BRANCH_ID, branchid, ledgertype);


            }
            else
            {

            }
        }


        private DataTable GetGeneralLedger2ndLevel(string FromDate, string ToDate, string ledger, string asondatewise, string BRANCH_ID, string HeadBranch, string ledgertype)
        {

            try
            {
                if (asondatewise == "Y")
                {
                    FromDate = ToDate;
                }
                DataSet ds = new DataSet();
                //SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]);
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("PRC_PROFIT_AND_LOSS_STATEMENT_DETAILED_REPORT", con);

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@COMPANYID", Convert.ToString(Session["LastCompany"]));
                cmd.Parameters.AddWithValue("@FINYEAR", Convert.ToString(Session["LastFinYear"]));
                cmd.Parameters.AddWithValue("@FROMDATE", FromDate);
                cmd.Parameters.AddWithValue("@TODATE", ToDate);
                cmd.Parameters.AddWithValue("@BRANCH_ID", BRANCH_ID);
                cmd.Parameters.AddWithValue("@AsonDate", asondatewise);
                cmd.Parameters.AddWithValue("@LEDGERID", ledger);
                cmd.Parameters.AddWithValue("@HO", HeadBranch);
                cmd.Parameters.AddWithValue("@LEGDERTYPE", ledgertype);
                cmd.Parameters.AddWithValue("@USERID", Convert.ToString(Session["userid"]).Trim());
                cmd.CommandTimeout = 0;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;

                da.Fill(ds);

                cmd.Dispose();
                con.Dispose();

                return ds.Tables[0];
            }
            catch (Exception ex)
            {
                return null;
            }
        }




        #endregion

        protected void drdExport_SelectedIndexChanged(object sender, EventArgs e)
        {


            Int32 Filter = int.Parse(Convert.ToString(1));
            PrintingSystemBase ps = new PrintingSystemBase();

            PrintableComponentLinkBase link1 = new PrintableComponentLinkBase(ps);
            link1.Component = ExpExporter;

            PrintableComponentLinkBase link2 = new PrintableComponentLinkBase(ps);
            link2.Component = IncomeExporter;



            CompositeLinkBase compositeLink = new CompositeLinkBase(ps);
            compositeLink.Links.AddRange(new object[] { link1, link2 });



            compositeLink.CreateDocument();

            using (MemoryStream stream = new MemoryStream())
            {
                XlsxExportOptions options = new XlsxExportOptions();
                options.TextExportMode = TextExportMode.Text;
                options.ExportMode = XlsxExportMode.SingleFile;

                if (Filter == 1)
                {

                    compositeLink.PrintingSystemBase.ExportToPdf(stream);
                    Response.Clear();
                    Response.Buffer = false;
                    Response.AppendHeader("Content-Type", "application/pdf");
                    Response.AppendHeader("Content-Transfer-Encoding", "binary");
                    Response.AppendHeader("Content-Disposition", "attachment; filename=Trading and PL Statement.pdf");
                    stream.Position = 0;
                    Response.BinaryWrite(stream.ToArray());
                    Response.End();
                }
                else if (Filter == 2)
                {

                    compositeLink.PrintingSystemBase.ExportToXlsx(stream, options);
                    Response.Clear();
                    Response.Buffer = false;
                    Response.AppendHeader("Content-Type", "application/xlsx");
                    Response.AppendHeader("Content-Transfer-Encoding", "binary");
                    Response.AppendHeader("Content-Disposition", "attachment; filename=Trading and PL Statement.xlsx");
                    stream.Position = 0;
                    Response.BinaryWrite(stream.ToArray());
                    Response.End();
                }
                else if (Filter == 3)
                {

                    compositeLink.PrintingSystemBase.ExportToRtf(stream);
                    Response.Clear();
                    Response.Buffer = false;
                    Response.AppendHeader("Content-Type", "application/rtf");
                    Response.AppendHeader("Content-Transfer-Encoding", "binary");
                    Response.AppendHeader("Content-Disposition", "attachment; filename=Trading and PL Statement.rtf");
                    stream.Position = 0;
                    Response.BinaryWrite(stream.ToArray());
                    Response.End();
                }
                else if (Filter == 4)
                {

                    compositeLink.PrintingSystemBase.ExportToCsv(stream);
                    Response.Clear();
                    Response.Buffer = false;
                    Response.AppendHeader("Content-Type", "application/csv");
                    Response.AppendHeader("Content-Transfer-Encoding", "binary");
                    Response.AppendHeader("Content-Disposition", "attachment; filename=Trading and PL Statement.csv");
                    stream.Position = 0;
                    Response.BinaryWrite(stream.ToArray());
                    Response.End();
                }



            }
            ps.Dispose();
        }

        protected void Component_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            string parameter = e.Parameter;

            if (parameter == "BindGrid")
            {
                Session["BSdata"] = null;
                TreeList.DataBind();
                TreeList.CollapseAll();
                if (chkPercentage.Checked)
                {
                    TreeList.Columns["Percentage"].Visible = true;
                }
                else
                {
                    TreeList.Columns["Percentage"].Visible = false;
                }
            }
            else if (parameter == "ExpandAll")
            {
                TreeList.ExpandAll();
            }
            else if (parameter == "CollapseAll")
            {
                TreeList.CollapseAll();
            }

            else if (parameter == "ShowPercentage")
            {
                TreeList.Columns["Percentage"].Visible = true;
            }
            else if (parameter == "HidePercentage")
            {
                TreeList.Columns["Percentage"].Visible = false;
            }

            if (parameter == "BindGrid")
            {
                //Session["TradingPLdata"] = null;
                TreeList1.DataBind();
                if (chkPercentage.Checked)
                {
                    TreeList1.Columns["Percentage"].Visible = true;
                }
                else
                {
                    TreeList1.Columns["Percentage"].Visible = false;
                }

                TreeList1.CollapseAll();
            }
            else if (parameter == "ExpandAll")
            {
                TreeList1.ExpandAll();
            }
            else if (parameter == "CollapseAll")
            {
                TreeList1.CollapseAll();
            }
            else if (parameter == "ShowPercentage")
            {
                TreeList1.Columns["Percentage"].Visible = true;
            }
            else if (parameter == "HidePercentage")
            {
                TreeList1.Columns["Percentage"].Visible = false;
            }
        }


        private DataTable GetGridDataSource(string FromDate, string ToDate, string ledger, string BRANCH_ID, bool Ason, string Entity_Id)
        {

            try
            {
                if (Session["BSZooming"] != null)
                {
                    return (DataTable)Session["BSZooming"];
                }
                else
                {
                    string project = "";
                    List<object> lstproject = lookup_project.GridView.GetSelectedFieldValues("ID");
                    project = string.Join(",", lstproject);
                    DataSet ds = new DataSet();

                    //SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]);
                    if (!string.IsNullOrEmpty(ledger))
                    {
                        SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                        SqlCommand cmd = new SqlCommand("prcr_BSZoomingProject", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Action ", "FirstLevel");
                        cmd.Parameters.AddWithValue("@COMPANYID", Convert.ToString(Session["LastCompany"]));
                        cmd.Parameters.AddWithValue("@FINYEAR", Convert.ToString(Session["LastFinYear"]));
                        cmd.Parameters.AddWithValue("@FROMDATE", FromDate);
                        cmd.Parameters.AddWithValue("@TODATE", ToDate);
                        cmd.Parameters.AddWithValue("@BRANCH_ID", BRANCH_ID);
                        cmd.Parameters.AddWithValue("@LEDGERID", ledger.Replace("M", ""));
                        cmd.Parameters.AddWithValue("@Ason", Ason);
                        cmd.Parameters.AddWithValue("@PROJECT_IDS", project);
                        cmd.Parameters.AddWithValue("@Entity_Id", Entity_Id);
                        cmd.Parameters.AddWithValue("@USERID", Convert.ToString(Session["userid"]).Trim());
                        cmd.CommandTimeout = 0;
                        SqlDataAdapter da = new SqlDataAdapter();
                        da.SelectCommand = cmd;

                        da.Fill(ds);

                        cmd.Dispose();
                        con.Dispose();

                        Session["BSZooming"] = ds.Tables[0];


                        return ds.Tables[0];
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private DataTable GetGridDataSourceFirstLevel(string FromDate, string ToDate, string ledger, string BRANCH_ID, bool Ason)
        {

            try
            {
                if (Session["BSZooming1"] != null)
                {
                    return (DataTable)Session["BSZooming1"];
                }
                else
                {
                    if (!string.IsNullOrEmpty(ledger))
                    {
                        string project = "";
                        List<object> lstproject = lookup_project.GridView.GetSelectedFieldValues("ID");
                        project = string.Join(",", lstproject);
                        DataSet ds = new DataSet();
                        //SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]);
                        SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                        SqlCommand cmd = new SqlCommand("prcr_BSZoomingProject", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Action ", "PreFirstLevel");
                        cmd.Parameters.AddWithValue("@COMPANYID", Convert.ToString(Session["LastCompany"]));
                        cmd.Parameters.AddWithValue("@FINYEAR", Convert.ToString(Session["LastFinYear"]));
                        cmd.Parameters.AddWithValue("@FROMDATE", FromDate);
                        cmd.Parameters.AddWithValue("@TODATE", ToDate);
                        cmd.Parameters.AddWithValue("@BRANCH_ID", BRANCH_ID);
                        cmd.Parameters.AddWithValue("@LEDGERID", ledger.Replace("M", ""));
                        cmd.Parameters.AddWithValue("@Ason", Ason);
                        cmd.Parameters.AddWithValue("@PROJECT_IDS", project);
                        cmd.Parameters.AddWithValue("@USERID", Convert.ToString(Session["userid"]).Trim());
                        cmd.CommandTimeout = 0;
                        SqlDataAdapter da = new SqlDataAdapter();
                        da.SelectCommand = cmd;

                        da.Fill(ds);

                        cmd.Dispose();
                        con.Dispose();

                        Session["BSZooming1"] = ds.Tables[0];


                        return ds.Tables[0];
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        protected void ShowGridDetails2Level_DataBinding(object sender, EventArgs e)
        {
            // Session["BSZooming"] = null;
            List<object> lstBranch = lookup_branch.GridView.GetSelectedFieldValues("ID");
            string branch = string.Join(",", lstBranch);
            ShowGridDetails2Level.DataSource = GetGridDataSource(ASPxFromDate.Date.ToString("yyyy-MM-dd"), ASPxToDate.Date.ToString("yyyy-MM-dd"), hdnLedger.Value, branch, radAsDate.Checked, hdnEntity_Id.Value);
        }

        protected void gvStockSummary_DataBinding(object sender, EventArgs e)
        {

            List<object> lstBranch = lookup_branch.GridView.GetSelectedFieldValues("ID");
            string branch = string.Join(",", lstBranch);
            if (hdn_LedgerCode.Value == "SYSTM00011")
                gvStockSummary.DataSource = GetGridDataSourceStockSummary(ASPxFromDate.Date.ToString("yyyy-MM-dd"), ASPxFromDate.Date.AddDays(-1).ToString("yyyy-MM-dd"), branch, radAsDate.Checked, ddlValTech.SelectedValue);
            else
                gvStockSummary.DataSource = GetGridDataSourceStockSummary(ASPxFromDate.Date.ToString("yyyy-MM-dd"), ASPxToDate.Date.ToString("yyyy-MM-dd"), branch, radAsDate.Checked, ddlValTech.SelectedValue);
        }


        protected void grivaluation_DataBinding(object sender, EventArgs e)
        {
            List<object> lstBranch = lookup_branch.GridView.GetSelectedFieldValues("ID");
            string branch = string.Join(",", lstBranch);
            if (hdnProductiD.Value != "")
            {
                if (hdn_LedgerCode.Value == "SYSTM00011")
                    gridStockDetials.DataSource = GetGridDataSourceStockDetails(ASPxFromDate.Date.ToString("yyyy-MM-dd"), ASPxFromDate.Date.AddDays(-1).ToString("yyyy-MM-dd"), hdnBranchId.Value, radAsDate.Checked, ddlValTech.SelectedValue, hdnProductiD.Value);
                else
                    gridStockDetials.DataSource = GetGridDataSourceStockDetails(ASPxFromDate.Date.ToString("yyyy-MM-dd"), ASPxToDate.Date.ToString("yyyy-MM-dd"), hdnBranchId.Value, radAsDate.Checked, ddlValTech.SelectedValue, hdnProductiD.Value);
            }
        }


        private DataTable GetGridDataSourceStockDetails(string FromDate, string Todate, string Branch, bool AsonDate, string Valuation, string productid)
        {
            try
            {
                if (Session["BSStockDetails"] != null)
                {
                    return (DataTable)Session["BSStockDetails"];
                }
                else
                {
                    if (!string.IsNullOrEmpty(productid))
                    {
                        string project = "";
                        List<object> lstproject = lookup_project.GridView.GetSelectedFieldValues("ID");
                        project = string.Join(",", lstproject);
                        DataSet ds = new DataSet();
                        //SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]);
                        SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                        SqlCommand cmd = new SqlCommand("prcr_BSZoomingProject", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Action ", "StockDetail");
                        cmd.Parameters.AddWithValue("@COMPANYID", Convert.ToString(Session["LastCompany"]));
                        cmd.Parameters.AddWithValue("@FINYEAR", Convert.ToString(Session["LastFinYear"]));
                        cmd.Parameters.AddWithValue("@FROMDATE", FromDate);
                        cmd.Parameters.AddWithValue("@TODATE", Todate);
                        cmd.Parameters.AddWithValue("@BRANCH_ID", Branch);
                        cmd.Parameters.AddWithValue("@Ason", AsonDate);
                        cmd.Parameters.AddWithValue("@Valuation", Valuation);
                        cmd.Parameters.AddWithValue("@PROJECT_IDS", project);
                        cmd.Parameters.AddWithValue("@Product", Convert.ToString(productid));
                        cmd.Parameters.AddWithValue("@chkConsiderOverhead", chkConsiderOverhead.Checked);
                        cmd.Parameters.AddWithValue("@USERID", Convert.ToString(Session["userid"]).Trim());
                        cmd.CommandTimeout = 0;
                        SqlDataAdapter da = new SqlDataAdapter();
                        da.SelectCommand = cmd;

                        da.Fill(ds);

                        cmd.Dispose();
                        con.Dispose();

                        Session["BSStockDetails"] = ds.Tables[0];
                        return ds.Tables[0];
                    }
                    else
                    {
                        return null;
                    }


                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }



        private DataTable GetGridDataSourceStockSummary(string FromDate, string Todate, string Branch, bool AsonDate, string Valuation)
        {
            try
            {
                if (Session["BSSTockSummary"] != null)
                {
                    return (DataTable)Session["BSSTockSummary"];
                }
                else
                {
                    if (!string.IsNullOrEmpty(Branch))
                    {
                        string project = "";
                        List<object> lstproject = lookup_project.GridView.GetSelectedFieldValues("ID");
                        project = string.Join(",", lstproject);
                        DataSet ds = new DataSet();
                        //SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]);
                        SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                        SqlCommand cmd = new SqlCommand("prcr_BSZoomingProject", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Action ", "StockSummary");
                        cmd.Parameters.AddWithValue("@COMPANYID", Convert.ToString(Session["LastCompany"]));
                        cmd.Parameters.AddWithValue("@FINYEAR", Convert.ToString(Session["LastFinYear"]));
                        cmd.Parameters.AddWithValue("@FROMDATE", FromDate);
                        cmd.Parameters.AddWithValue("@TODATE", Todate);
                        cmd.Parameters.AddWithValue("@BRANCH_ID", Branch);
                        cmd.Parameters.AddWithValue("@Ason", AsonDate);
                        cmd.Parameters.AddWithValue("@Valuation", Valuation);
                        cmd.Parameters.AddWithValue("@PROJECT_IDS", project);
                        cmd.Parameters.AddWithValue("@chkConsiderOverhead", chkConsiderOverhead.Checked);
                        cmd.Parameters.AddWithValue("@USERID", Convert.ToString(Session["userid"]).Trim());
                        cmd.CommandTimeout = 0;
                        SqlDataAdapter da = new SqlDataAdapter();
                        da.SelectCommand = cmd;

                        da.Fill(ds);

                        cmd.Dispose();
                        con.Dispose();

                        Session["BSSTockSummary"] = ds.Tables[0];


                        return ds.Tables[0];
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        protected void ShowGridDetails1Level_DataBinding(object sender, EventArgs e)
        {
            // Session["BSZooming"] = null;
            List<object> lstBranch = lookup_branch.GridView.GetSelectedFieldValues("ID");
            string branch = string.Join(",", lstBranch);
            ShowGridDetails1Level.DataSource = GetGridDataSourceFirstLevel(ASPxFromDate.Date.ToString("yyyy-MM-dd"), ASPxToDate.Date.ToString("yyyy-MM-dd"), hdnLedger.Value, branch, radAsDate.Checked);
        }
        protected void cclStockDetails_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
        {
            Session["BSStockDetails"] = null;
            gridStockDetials.DataBind();
        }
        protected void cclStockSummary_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            Session["BSSTockSummary"] = null;
            gvStockSummary.DataBind();
        }
        protected void cCallbackPanelDetail_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            Session["BSZooming"] = null;
            ShowGridDetails2Level.DataBind();
        }

        protected void cCallbackPanelDetail1_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            Session["BSZooming1"] = null;
            ShowGridDetails1Level.DataBind();
        }
        protected void ShowGridDetails2Level_SummaryDisplayText(object sender, ASPxGridViewSummaryDisplayTextEventArgs e)
        {
            e.Text = string.Format("{0}", e.Value);
        }


        protected void ddlExport3_SelectedIndexChanged(object sender, EventArgs e)
        {
            Int32 Filter = int.Parse(Convert.ToString(Session["ddlExport3"]));
            Session["ddlExport3"] = "0";

            DropDownList dl = sender as DropDownList;
            dl.ClearSelection(); //making sure the previous selection has been cleared
            dl.SelectedValue = "0";

            if (Filter != 0)
            {
                bindexport_Details3(Filter);
            }
            //ShowGridDetails1Level.DataBind();
            //gvStockSummary.DataBind();
            //gridStockDetials.DataBind();
            //ShowGridDetails2Level.DataBind();

            //this.Page.ClientScript.RegisterStartupScript(GetType(), "setCookieOnStorage", "ClosePopUpstkDetLev2(null,null);");

        }
        public void bindexport_Details3(int Filter)
        {
            string filename = "Balance Sheet Detail";
            exporterDetails.FileName = filename;
            exporterDetails.FileName = "Balance Sheet Detail";

            string FileHeader = "";
            BusinessLogicLayer.Reports RptHeader = new BusinessLogicLayer.Reports();
            FileHeader = RptHeader.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), true, true, true, true, true, true) + Environment.NewLine + "Balance Sheet Detail" + Environment.NewLine + "For the period " + Convert.ToDateTime(ASPxFromDate.Date).ToString("dd-MM-yyyy") + " To " + Convert.ToDateTime(ASPxToDate.Date).ToString("dd-MM-yyyy");
            exporterDetails.RenderBrick += exporter_RenderBrick;

            exporterDetails.PageHeader.Left = FileHeader;
            exporterDetails.PageHeader.Font.Size = 10;
            exporterDetails.PageHeader.Font.Name = "Tahoma";
            exporterDetails.PageFooter.Center = "[Page # of Pages #]";
            exporterDetails.PageFooter.Right = "[Date Printed]";
            exporterDetails.GridViewID = "ShowGridDetails2Level";
            switch (Filter)
            {
                case 1:
                    exporterDetails.WritePdfToResponse();
                    break;
                case 2:
                    exporterDetails.WriteXlsxToResponse(new XlsxExportOptionsEx() { ExportType = ExportType.WYSIWYG });
                    break;
                case 3:
                    exporterDetails.WriteRtfToResponse();
                    break;
                case 4:
                    exporterDetails.WriteCsvToResponse();
                    break;

                default:
                    return;
            }
        }
        protected void exporter_RenderBrick(object sender, ASPxGridViewExportRenderingEventArgs e)
        {
            e.BrickStyle.BackColor = Color.White;
            e.BrickStyle.ForeColor = Color.Black;
        }


        [WebMethod]
        public static string GetLedgerCode(string LedgerId)
        {
            try
            {
                DataTable ds = new DataTable();
                //SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionDefault"]);
                SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
                SqlCommand cmd = new SqlCommand("prcr_BSZoomingProject", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action ", "getledgercode");
                cmd.Parameters.AddWithValue("@LedgerId", Convert.ToString(LedgerId).Replace("M", ""));
                cmd.CommandTimeout = 0;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;

                da.Fill(ds);

                cmd.Dispose();
                con.Dispose();

                if (ds != null && ds.Rows.Count > 0)
                {
                    return Convert.ToString(ds.Rows[0]["ledgerCode"]);
                }
                else
                {
                    return null;
                }



            }
            catch (Exception ex)
            {
                return null;
            }
        }

        protected void ddlExport4_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Int32 Filter = int.Parse(Convert.ToString(ddlExport4.SelectedItem.Value));
            //DropDownList dl = sender as DropDownList;
            //dl.ClearSelection(); //making sure the previous selection has been cleared
            //dl.SelectedValue = "0";


            //ShowGridDetails1Level.DataBind();

            //if (Filter != 0)
            //{
            //    bindexport_Details4(Filter);
            //}





        }

        public void bindexport_Details4(int Filter)
        {
            string filename = "Balance Sheet Statement Detail";
            Exporter1.FileName = filename;
            Exporter1.FileName = "Balance Sheet Statement Detail";

            string FileHeader = "";
            BusinessLogicLayer.Reports RptHeader = new BusinessLogicLayer.Reports();
            FileHeader = RptHeader.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), true, true, true, true, true, true) + Environment.NewLine + "Balance Sheet Statement Detail" + Environment.NewLine + "For the period " + Convert.ToDateTime(ASPxFromDate.Date).ToString("dd-MM-yyyy") + " To " + Convert.ToDateTime(ASPxToDate.Date).ToString("dd-MM-yyyy");
            Exporter1.RenderBrick += exporter_RenderBrick;

            Exporter1.PageHeader.Left = FileHeader;
            Exporter1.PageHeader.Font.Size = 10;
            Exporter1.PageHeader.Font.Name = "Tahoma";
            Exporter1.PageFooter.Center = "[Page # of Pages #]";
            Exporter1.PageFooter.Right = "[Date Printed]";
            Exporter1.GridViewID = "ShowGridDetails1Level";
            switch (Filter)
            {
                case 1:
                    Exporter1.WritePdfToResponse();
                    break;
                case 2:
                    Exporter1.WriteXlsxToResponse(new XlsxExportOptionsEx() { ExportType = ExportType.WYSIWYG });
                    break;
                case 3:
                    Exporter1.WriteRtfToResponse();
                    break;
                case 4:
                    Exporter1.WriteCsvToResponse();
                    break;

                default:
                    return;
            }
        }

        protected void ddlExport5_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Int32 Filter = int.Parse(Convert.ToString(ddlExport5.SelectedItem.Value));

            //DropDownList dl = sender as DropDownList;
            //dl.ClearSelection(); //making sure the previous selection has been cleared
            //dl.SelectedValue = "0";


            //if (Filter != 0)
            //{
            //    bindexport_Details5(Filter);
            //}
            //ShowGridDetails1Level.DataBind();
            //gvStockSummary.DataBind();
            //gridStockDetials.DataBind();
            //ShowGridDetails2Level.DataBind();

            //this.Page.ClientScript.RegisterStartupScript(GetType(), "setCookieOnStorage", "ClosePopUpstkSum(null,null);");
        }

        public void bindexport_Details5(int Filter)
        {
            string filename = "Balance Sheet Statement Detail";
            SummaryExporter.FileName = filename;
            SummaryExporter.FileName = "Balance Sheet Statement Detail";

            string FileHeader = "";
            BusinessLogicLayer.Reports RptHeader = new BusinessLogicLayer.Reports();
            FileHeader = RptHeader.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), true, true, true, true, true, true) + Environment.NewLine + "Balance Sheet Statement Detail" + Environment.NewLine + "For the period " + Convert.ToDateTime(ASPxFromDate.Date).ToString("dd-MM-yyyy") + " To " + Convert.ToDateTime(ASPxToDate.Date).ToString("dd-MM-yyyy");
            SummaryExporter.RenderBrick += exporter_RenderBrick;

            SummaryExporter.PageHeader.Left = FileHeader;
            SummaryExporter.PageHeader.Font.Size = 10;
            SummaryExporter.PageHeader.Font.Name = "Tahoma";
            SummaryExporter.PageFooter.Center = "[Page # of Pages #]";
            SummaryExporter.PageFooter.Right = "[Date Printed]";
            SummaryExporter.GridViewID = "gvStockSummary";
            switch (Filter)
            {
                case 1:
                    SummaryExporter.WritePdfToResponse();
                    break;
                case 2:
                    SummaryExporter.WriteXlsxToResponse(new XlsxExportOptionsEx() { ExportType = ExportType.WYSIWYG });
                    break;
                case 3:
                    SummaryExporter.WriteRtfToResponse();
                    break;
                case 4:
                    SummaryExporter.WriteCsvToResponse();
                    break;

                default:
                    return;
            }
        }

        protected void ddlExport6_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Int32 Filter = int.Parse(Convert.ToString(ddlExport6.SelectedItem.Value));


            //DropDownList dl = sender as DropDownList;
            //dl.ClearSelection(); //making sure the previous selection has been cleared
            //dl.SelectedValue = "0";

            //if (Filter != 0)
            //{
            //    bindexport_Details6(Filter);
            //}
            //ShowGridDetails1Level.DataBind();
            //gvStockSummary.DataBind();
            //gridStockDetials.DataBind();
            //ShowGridDetails2Level.DataBind();

            //this.Page.ClientScript.RegisterStartupScript(GetType(), "setCookieOnStorage", "ClosePopUpstkDet(null,null);");
        }

        public void bindexport_Details6(int Filter)
        {
            string filename = "Balance Sheet Statement Detail";
            DetailsExporter.FileName = filename;
            DetailsExporter.FileName = "Balance Sheet Statement Detail";

            string FileHeader = "";
            BusinessLogicLayer.Reports RptHeader = new BusinessLogicLayer.Reports();
            FileHeader = RptHeader.CommonReportHeader(Convert.ToString(Session["LastCompany"]), Convert.ToString(Session["LastFinYear"]), true, true, true, true, true, true) + Environment.NewLine + "Balance Sheet Statement Detail" + Environment.NewLine + "For the period " + Convert.ToDateTime(ASPxFromDate.Date).ToString("dd-MM-yyyy") + " To " + Convert.ToDateTime(ASPxToDate.Date).ToString("dd-MM-yyyy");
            DetailsExporter.RenderBrick += exporter_RenderBrick;

            DetailsExporter.PageHeader.Left = FileHeader;
            DetailsExporter.PageHeader.Font.Size = 10;
            DetailsExporter.PageHeader.Font.Name = "Tahoma";
            DetailsExporter.PageFooter.Center = "[Page # of Pages #]";
            DetailsExporter.PageFooter.Right = "[Date Printed]";
            DetailsExporter.GridViewID = "gridStockDetials";
            switch (Filter)
            {
                case 1:
                    DetailsExporter.WritePdfToResponse();
                    break;
                case 2:
                    DetailsExporter.WriteXlsxToResponse(new XlsxExportOptionsEx() { ExportType = ExportType.WYSIWYG });
                    break;
                case 3:
                    DetailsExporter.WriteRtfToResponse();
                    break;
                case 4:
                    DetailsExporter.WriteCsvToResponse();
                    break;

                default:
                    return;
            }
        }

        protected void Unnamed_Click(object sender, EventArgs e)
        {
            TreeList.DataBind();
            TreeList1.DataBind();



            PrintPLBS report = new PrintPLBS();

            report.printableComponentContainer1.PrintableComponent = ExpExporter;
            report.printableComponentContainer2.PrintableComponent = IncomeExporter;

            //report.PageWidth = 50000;
            //report.PaperName = "A6";
            report.ShowPrintStatusDialog = true;
            report.txtCompanyName.Text = "Amith Marble";

            // Assemble the report document and publish it 
            // (e.g., save it to a memory stream in a third-party format).
            using (MemoryStream stream = new MemoryStream())
            {
                if (ExportTypeName.Value == "Excel")
                {
                    report.CreateDocument();
                    report.Landscape = true;
                    report.ExportToXlsx(stream);
                    Response.ClearContent();
                    Response.Buffer = true;
                    //Response.ContentType = "application/pdf";
                    //Response.AddHeader("content-disposition", "attachment;filename=Test.pdf");
                    //Response.Cache.SetCacheability(HttpCacheability.NoCache);  
                    //Response.ContentType = "application/pdf";
                    byte[] buffer = stream.ToArray();
                    //Response.OutputStream.Write(buffer, 0, (int)stream.Length);
                    //Response.Write("BS.xlsx");
                    Response.Clear();
                    Response.ContentType = "application/force-download";
                    Response.AddHeader("content-disposition", "attachment;    filename=BS.xlsx");
                    Response.BinaryWrite(buffer);
                    Response.End();
                }
                else if (ExportTypeName.Value == "Pdf")
                {
                    report.CreateDocument();
                    report.Landscape = true;
                    report.ExportToPdf(stream);
                    Response.ClearContent();
                    Response.Buffer = true;
                    //Response.ContentType = "application/pdf";
                    //Response.AddHeader("content-disposition", "attachment;filename=Test.pdf");
                    //Response.Cache.SetCacheability(HttpCacheability.NoCache);  
                    //Response.ContentType = "application/pdf";
                    byte[] buffer = stream.ToArray();
                    //Response.OutputStream.Write(buffer, 0, (int)stream.Length);
                    //Response.Write("BS.xlsx");
                    Response.Clear();
                    Response.ContentType = "application/force-download";
                    Response.AddHeader("content-disposition", "attachment;    filename=BS.pdf");
                    Response.BinaryWrite(buffer);
                    Response.End();
                }
                else if (ExportTypeName.Value == "Csv")
                {
                    report.CreateDocument();
                    report.Landscape = true;
                    report.ExportToCsv(stream);
                    Response.ClearContent();
                    Response.Buffer = true;
                    //Response.ContentType = "application/pdf";
                    //Response.AddHeader("content-disposition", "attachment;filename=Test.pdf");
                    //Response.Cache.SetCacheability(HttpCacheability.NoCache);  
                    //Response.ContentType = "application/pdf";
                    byte[] buffer = stream.ToArray();
                    //Response.OutputStream.Write(buffer, 0, (int)stream.Length);
                    //Response.Write("BS.xlsx");
                    Response.Clear();
                    Response.ContentType = "application/force-download";
                    Response.AddHeader("content-disposition", "attachment;    filename=BS.csv");
                    Response.BinaryWrite(buffer);
                    Response.End();
                }
                else
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        PrintPLBS reports = new PrintPLBS();

                        reports.printableComponentContainer1.PrintableComponent = ExpExporter;
                        reports.printableComponentContainer2.PrintableComponent = IncomeExporter;

                        //report.PageWidth = 50000;
                        //report.PaperName = "A6";
                        reports.ShowPrintStatusDialog = true;
                        reports.txtCompanyName.Text = "Amith Marble";
                        //reports.CreateDocument();
                        Session["report"] = report;

                        ExportTypeName.Value = "OpenPrintNewTab";

                        // Response.Write(String.Format("window.open('{0}','_blank')", ResolveUrl("PLBSViewer.aspx")));

                        // String clientScriptName = "ButtonClickScript";
                        //Type clientScriptType = this.GetType();
                        //ClientScriptManager clientScript = Page.ClientScript;

                        //// Check to see if the client script is already registered.
                        //if (!clientScript.IsClientScriptBlockRegistered(clientScriptType, clientScriptName))
                        //{
                        //    StringBuilder sb = new StringBuilder();
                        //    sb.Append("<script type='text/javascript'>");
                        //    sb.Append("window.open('PLBSViewer.aspx')"); //URL = where you want to redirect.
                        //    sb.Append("</script>");
                        //    clientScript.RegisterClientScriptBlock(clientScriptType, clientScriptName, sb.ToString());
                        //}


                        //ReportPrintTool printTool = new ReportPrintTool(report);
                        //printTool.PrintDialog();
                        //printTool.ShowPreview();
                        // Send the report to the default printer.
                        //printTool.Print();
                        // Send the report to the specified printer.
                        // printTool.PrintPR


                        //reports.CreateDocument();
                        //PdfExportOptions opts = new PdfExportOptions();
                        //opts.ShowPrintDialogOnOpen = true;
                        //reports.ExportToPdf(ms, opts);
                        //ms.Seek(0, SeekOrigin.Begin);
                        //byte[] reportz = ms.ToArray();
                        //Page.Response.ContentType = "application/pdf";
                        //Page.Response.Clear();
                        //Page.Response.OutputStream.Write(reportz, 0, reportz.Length);
                        //Page.Response.End();
                    }
                }


            }

        }

        protected void FirstImport_Click(object sender, EventArgs e)
        {
            bindexport_Details4(2);
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            bindexport_Details5(2);
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            bindexport_Details6(2);
        }

        protected void Button3_Click(object sender, EventArgs e)
        {
            bindexport_Details3(2);
        }



        private void SendToPrinter()
        {
            ProcessStartInfo info = new ProcessStartInfo();
            info.Verb = "print";
            info.FileName = "BS.pdf";
            info.CreateNoWindow = true;
            info.WindowStyle = ProcessWindowStyle.Hidden;

            Process p = new Process();
            p.StartInfo = info;
            p.Start();

            p.WaitForInputIdle();
            System.Threading.Thread.Sleep(3000);
            if (false == p.CloseMainWindow())
                p.Kill();
        }



        protected void btnSaveRecords_Click(object sender, EventArgs e)
        {
            PrintPLBS reports = new PrintPLBS();
            reports.printableComponentContainer1.PrintableComponent = ExpExporter;
            reports.printableComponentContainer2.PrintableComponent = IncomeExporter;
            reports.ShowPrintStatusDialog = true;
            reports.txtCompanyName.Text = "Amith Marble";
            //reports.CreateDocument();
            Session["report"] = reports;
            ExportTypeName.Value = "OpenPrintNewTab";

            Response.ClearContent();
            //Response.Buffer = true;
            Response.Clear();
            //Response.End();
        }

        protected void Button6_Click(object sender, EventArgs e)
        {
            TreeList.ExpandAll();
            TreeList1.ExpandAll();


            PrintPLBS reports = new PrintPLBS();
            reports.printableComponentContainer1.PrintableComponent = IncomeExporter;
            reports.printableComponentContainer2.PrintableComponent = ExpExporter;
            reports.ShowPrintStatusDialog = true;

            //reports.CreateDocument();


            DataTable dtCompany = oDBEngine.GetDataTable("select cmp_Name,add_address1 +','+add_address2+','+add_address3 Address1,city.city_name + ' - ' + pinzip.pin_code Address2 from tbl_master_company com inner join  tbl_master_address addr on com.cmp_internalid=addr.add_cntId inner join tbl_master_city city on city.city_id=addr.add_city inner join tbl_master_pinzip pinzip on pinzip.pin_id=addr.add_pin where add_cntid='" + Convert.ToString(Session["LastCompany"]) + "' and add_addressType='Billing'");

            if (dtCompany != null && dtCompany.Rows.Count > 0)
            {
                reports.txtCompanyName.Text = Convert.ToString(dtCompany.Rows[0]["cmp_Name"]);
                reports.AddressLine1.Text = Convert.ToString(dtCompany.Rows[0]["Address1"]);
                reports.AddressLine2.Text = Convert.ToString(dtCompany.Rows[0]["Address2"]);

            }

            reports.TypeLeft.Text = "L I A B I L I T I E S";
            reports.TypeRight.Text = "A S S E T S";

            if (radAsDate.Checked)
            {
                reports.AsOn.Text = "Balance Sheet  As At " + ASPxToDate.Date.ToString("dd-MM-yyyy");
            }
            else
            {
                reports.AsOn.Text = "Balance Sheet Period : " + ASPxFromDate.Date.ToString("dd-MM-yyyy") + " - " + ASPxToDate.Date.ToString("dd-MM-yyyy");
            }


            Session["report"] = reports;
            ExportTypeName.Value = "OpenPrintNewTab";

            Response.ClearContent();
            //Response.Buffer = true;
            Response.Clear();
            Response.Redirect("PLBSViewer.aspx?Key=BSStatement");

            //Response.End();
        }

        protected void Project_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            if (e.Parameter.Split('~')[0] == "BindProjectGrid")
            {
                DataTable ProjectTable = new DataTable();
                ProjectTable = GetProject();

                if (ProjectTable.Rows.Count > 0)
                {
                    Session["ProjectData"] = ProjectTable;
                    lookup_project.DataSource = ProjectTable;
                    lookup_project.DataBind();
                }
                else
                {
                    Session["ProjectData"] = ProjectTable;
                    lookup_project.DataSource = null;
                    lookup_project.DataBind();
                }
            }
        }

        public DataTable GetProject()
        {
            DataTable dt = new DataTable();
            SqlConnection con = new SqlConnection(Convert.ToString(System.Web.HttpContext.Current.Session["ErpConnection"]));
            SqlCommand cmd = new SqlCommand("PRC_FETCHPROJECTS_REPORT", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 0;
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            da.Fill(dt);
            cmd.Dispose();
            con.Dispose();

            return dt;
        }

        protected void lookup_project_DataBinding(object sender, EventArgs e)
        {
            if (Session["ProjectData"] != null)
            {
                lookup_project.DataSource = (DataTable)Session["ProjectData"];
            }
        }

    }
}